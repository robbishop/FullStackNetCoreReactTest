using Amazon.CDK;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.S3;
using Constructs;
using System.Collections.Generic;

namespace Core_Web_Api.Deploy
{
    public class CoreWebApiDeploy : Stack
    {
        internal CoreWebApiDeploy(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            const string httpApiName = "CoreWebApiService";

            var exeRole = new Role(this, $"{httpApiName}-ExeRole", new RoleProps
            {
                RoleName = $"{httpApiName}-ExeRole",
                AssumedBy = new ServicePrincipal("lambda.amazonaws.com"),
                ManagedPolicies = new[] { ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSLambdaBasicExecutionRole") }
            });

            new Amazon.CDK.AWS.Lambda.Function(this, httpApiName, new Amazon.CDK.AWS.Lambda.FunctionProps
            {
                Runtime = Runtime.DOTNET_6,
                Code = Code.FromAsset("../BackEnd/Core-Web-Api/bin/Release/net6.0/Core-Web-Api.zip"),
                FunctionName = httpApiName,
                Role = exeRole,
                MemorySize = 512,
                Handler = "Core-Web-Api",
                Timeout = Duration.Seconds(10),
                LogRetention = RetentionDays.ONE_MONTH,
            })
            .AddFunctionUrl(new FunctionUrlOptions
            {
                AuthType = FunctionUrlAuthType.NONE,
                Cors = new FunctionUrlCorsOptions
                {
                    AllowedOrigins = new[] { "*" },
                    AllowedHeaders = new[] { "*" },
                    AllowedMethods = new[] { HttpMethod.ALL },
                    MaxAge = Duration.Seconds(0),
                }
            });

            var bucket = new Bucket(this, "CoreWebApiBucket", new BucketProps
            {
                BucketName = "gbg-core-web-api-dsas0u398",
            });

            var ctrl = new CfnOriginAccessControl(this, "CoreWebApiDistroControl", new CfnOriginAccessControlProps
            {
                OriginAccessControlConfig = new CfnOriginAccessControl.OriginAccessControlConfigProperty
                {
                    Name = "CoreWebApiBucket",
                    OriginAccessControlOriginType = "s3",
                    SigningBehavior = "always",
                    SigningProtocol = "sigv4"
                }
            });

            var basicAuthOnDistro = new Amazon.CDK.AWS.CloudFront.Function(this, "CoreWebApiDistroBasicAuthFn", new Amazon.CDK.AWS.CloudFront.FunctionProps
            {
                Code = FunctionCode.FromInline(@"function handler(event) {
                  var authHeaders = event.request.headers.authorization;
                  var expected = ""Basic Z2JnOmludGVydmlldzJfc2Fmd3ExMjM="";
                  if (authHeaders && authHeaders.value === expected) {
                    return event.request;
                  }
                  var response = {
                    statusCode: 401,
                    statusDescription: ""Unauthorized"",
                    headers: {
                      ""www-authenticate"": {
                        value: 'Basic realm=""Enter credentials""',
                      },
                    },
                  };
                  return response;
                }"),
                FunctionName = "CoreWebApiDistroBasicAuthFn"
            });

            var distro = new CfnDistribution(this, "CoreWebApiDistro", new CfnDistributionProps
            {
                DistributionConfig = new CfnDistribution.DistributionConfigProperty
                {
                    DefaultCacheBehavior = new CfnDistribution.DefaultCacheBehaviorProperty
                    {
                        TargetOriginId = "S3Origin",
                        ViewerProtocolPolicy = "redirect-to-https",
                        CachePolicyId = "658327ea-f89d-4fab-a63d-7e88639e58f6",
                        FunctionAssociations = new[]
                        {
                            new CfnDistribution.FunctionAssociationProperty
                            {
                                EventType = "viewer-request",
                                FunctionArn = basicAuthOnDistro.FunctionArn
                            }
                        }
                    },
                    Enabled = true,
                    Origins = new[]
                    {
                        new CfnDistribution.OriginProperty
                        {
                            Id = "S3Origin",
                            S3OriginConfig = new CfnDistribution.S3OriginConfigProperty
                            {
                                OriginAccessIdentity = ""
                            },
                            OriginAccessControlId = ctrl.GetAtt("Id").ToString(),
                            DomainName = bucket.BucketDomainName,
                        }
                    },
                    DefaultRootObject = "index.html",
                },
            });

            var bucketPolicyStatement = new PolicyStatement(new PolicyStatementProps
            {
                Actions = new[] { "s3:GetObject" },
                Effect = Effect.ALLOW,
                Resources = new[] { $"{bucket.BucketArn}/*" }
            });
            bucketPolicyStatement.AddCondition("StringEquals", new Dictionary<string, string> { { "AWS:SourceArn", $"arn:aws:cloudfront::{Account}:distribution/{distro.GetAtt("Id")}" } });
            bucketPolicyStatement.AddServicePrincipal("cloudfront.amazonaws.com");

            bucket.AddToResourcePolicy(bucketPolicyStatement);
        }
    }
}
