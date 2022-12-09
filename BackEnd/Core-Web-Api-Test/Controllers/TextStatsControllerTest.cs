using Core_Web_Api_Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Core_Web_Api_Test.Controllers
{
    public class TextStatsControllerTest
    {
        private readonly HttpClient _client;

        public TextStatsControllerTest()
        {
            _client = new WebApplicationFactory<Program>().CreateClient();
        }

        [Fact]
        public async Task Returns_stats_result_as_text_from_request_as_json_string()
        {
            var content = new StringContent(@"""one two three""", Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "api/TextStats")
            {
                Content = content
            };
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
            var result = await _client.SendAsync(request);

            result.EnsureSuccessStatusCode();
            var statsResult = await result.Content.ReadAsStringAsync();

            const string expected = "Character count: 11\r\nLine count: 1\r\nParagraph count: 1\r\nSentence count: 1\r\n";
            Assert.Equal(expected, statsResult);
        }

        [Fact]
        public async Task Returns_stats_result_as_json_from_request_as_json_string()
        {
            var content = new StringContent(@"""one two three""", Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "api/TextStats")
            {
                Content = content
            };
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            var result = await _client.SendAsync(request);

            result.EnsureSuccessStatusCode();
            var statsResult = await result.Content.ReadFromJsonAsync<TextStatisticResult>();

            Assert.Equal(11, statsResult?.CharacterCount);
        }

        [Fact]
        public async Task Returns_stats_result_as_json_from_request_as_file()
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.SimpleEnglishRequest));
            using var formContent = new MultipartFormDataContent();

            var fileContent = new StreamContent(stream);
            fileContent.Headers.Add("Content-Type", "text/plain");
            formContent.Add(fileContent, "file", "filename");

            var result = await _client.PostAsync("api/TextStats", formContent);
            result.EnsureSuccessStatusCode();
            var statsResult = await result.Content.ReadFromJsonAsync<TextStatisticResult>();

            Assert.Equal(10, statsResult?.SentenceCount);
        }


        [Fact]
        public async Task Returns_BadRequest_when_no_content_is_supplied_in_form_data()
        {
            using var formContent = new MultipartFormDataContent();

            var result = await _client.PostAsync("api/TextStats", formContent);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Returns_UnsupportedMediaType_when_supplied_file_is_not_text()
        {
            var buffer = new byte[100];
            using var stream = new MemoryStream(buffer);
            using var formContent = new MultipartFormDataContent();

            var fileContent = new StreamContent(stream);
            fileContent.Headers.Add("Content-Type", "application/octet-stream");
            formContent.Add(fileContent, "file", "filename");

            var result = await _client.PostAsync("api/TextStats", formContent);
            var problem = await result.Content.ReadFromJsonAsync<ProblemDetails>();

            Assert.Equal(HttpStatusCode.UnsupportedMediaType, result.StatusCode);
            Assert.Contains("unsupported file type", problem?.Detail, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Returns_BadRequest_when_supplied_file_size_is_too_big()
        {
            const int maxAllowed = 1024 * 1024;
            const int fileSize = maxAllowed + 1;
            var buffer = new byte[fileSize];
            using var stream = new MemoryStream(buffer);
            using var formContent = new MultipartFormDataContent();

            var fileContent = new StreamContent(stream);
            fileContent.Headers.Add("Content-Type", "text/plain");
            formContent.Add(fileContent, "file", "filename");

            var result = await _client.PostAsync("api/TextStats", formContent);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
