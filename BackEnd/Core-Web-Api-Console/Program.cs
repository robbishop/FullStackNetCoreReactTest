// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Core_Web_Api_Console;
using Core_Web_Api_Text;
using System.Text;
using Testing;


GetInfo(string.Concat(Enumerable.Repeat(Resources.SampleString, 10)));
GetInfo(string.Concat(Enumerable.Repeat(Resources.SampleString, 100)));
GetInfo(string.Concat(Enumerable.Repeat(Resources.SampleString, 1000)));
GetInfo(string.Concat(Enumerable.Repeat(Resources.SampleString, 10000)));
GetInfo(string.Concat(Enumerable.Repeat(Resources.SampleString, 100000)));

void GetInfo(string s)
{
    var bytes = Encoding.UTF8.GetBytes(s);
    Console.WriteLine(s.Length + " " + bytes.Length);
}

//var summary = BenchmarkRunner.Run<GetTextStatBenchmark>();
//Console.WriteLine(summary);

namespace Testing
{
    [MemoryDiagnoser]
    public class GetTextStatBenchmark
    {
        [Benchmark]
        [ArgumentsSource(nameof(DataSource))]
        public void GetAllStats(Sample sample)
        {
            var svc = new TextStatisticServiceRefactor();
            svc.LoadString(sample.Value);
            svc.GetAllStats();
        }

        public static IEnumerable<Sample> DataSource()
        {
            yield return new Sample(10, string.Concat(Enumerable.Repeat(Resources.SampleString, 10)));
            yield return new Sample(100, string.Concat(Enumerable.Repeat(Resources.SampleString, 100)));
            yield return new Sample(1000, string.Concat(Enumerable.Repeat(Resources.SampleString, 1000)));
            yield return new Sample(10000, string.Concat(Enumerable.Repeat(Resources.SampleString, 10000)));
            yield return new Sample(100000, string.Concat(Enumerable.Repeat(Resources.SampleString, 100000)));
        }

        public class Sample
        {
            private readonly long multiplier;

            public Sample(long multiplier, string initialValue)
            {
                this.multiplier = multiplier;
                Value = initialValue;
            }

            public string Value { get; }

            public override string ToString() => multiplier.ToString();
        }
    }

    public class Sampler
    {
        [ParamsSource(nameof(ValuesForInputText))]
        public string InputText { get; set; } = "";

        private readonly TextStatisticService svc = new();

        public IEnumerable<string> ValuesForInputText { get; } = Enumerable.Empty<string>();

        [GlobalSetup]
        public static void Setup()
        {
            const string baseString = "!every Place hath yielding. Light shall said from isn't?";
            var inputs = new List<string>
            {
                "10 " + baseString
            };
        }

        [IterationSetup]
        public void SetupService()
        {
            Console.WriteLine("!!!!!!!!! Loading Text " + InputText);
            var svc = new TextStatisticService();
            svc.LoadString(InputText);
        }

        [Benchmark(Description = "GetCharacterCount")]
        public void Benchmark()
        {
            Console.WriteLine("!!!!!!!!! Benchmark Text " + InputText);
            svc.GetAllStats();
        }

        //private readonly SHA256 sha256 = SHA256.Create();
        //private readonly MD5 md5 = MD5.Create();
        //private byte[] data = Array.Empty<byte>();

        //[Params(1000, 10000)]
        //public int N;

        //[GlobalSetup]
        //public void Setup()
        //{
        //    data = new byte[N];
        //    new Random(42).NextBytes(data);
        //}

        //[Benchmark]
        //public byte[] Sha256() => sha256.ComputeHash(data);

        //[Benchmark]
        //public byte[] Md5() => md5.ComputeHash(data);
    }
}