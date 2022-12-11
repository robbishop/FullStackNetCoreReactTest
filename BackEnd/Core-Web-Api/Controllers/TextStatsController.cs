using Core_Web_Api_Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Core_Web_Api.Controllers
{
    [Route("api/TextStats")]
    [ApiController]
    public class TextStatsController : ControllerBase
    {
        private readonly ITextStatisticService _service;

        public TextStatsController(ITextStatisticService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return new ContentResult { Content = "{ \"status\": \"OK\" }", ContentType = "application/json" };
        }

        /// <summary>
        /// Gets statistical details from a string provided
        /// </summary>
        /// <param name="requestedText">The text to be analyzed.</param>
        /// <returns>A set of statistics</returns>
        [HttpPost(Name = "GetTextStats")]
        [RequestSizeLimit(1024)]
        public ActionResult GetTextStats([FromBody] string requestedText)
        {
            _service.LoadString(requestedText);

            var result = _service.GetAllStats();
            if (!Request.Headers.Accept.Contains("application/json")
                && Request.Headers.Accept.Contains("text/plain"))
            {
                return new ContentResult
                {
                    Content = result.ToString(),
                    ContentType = "text/plain",
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
            return Ok(result);
        }

        /// <summary>
        /// Gets statistical details from a file provided
        /// </summary>
        /// <returns>A set of statistics</returns>
        [HttpPost(Name = "GetTextFileStats"), Consumes("multipart/form-data")]
        [RequestFormLimits(MultipartBodyLengthLimit = 1048576)] // = 1024*1024 = 1MB
        public async Task<ActionResult> GetTextFileStats(IFormFile file)
        {
            if (file is null)
                return Problem(detail: "No file supplied", statusCode: (int)HttpStatusCode.BadRequest);

            if (file.ContentType != "text/plain")
                return Problem(detail: $"Unsupported file type {Request.Form?.Files[0].ContentType}, please supply a text file (text/plain)", statusCode: (int)HttpStatusCode.UnsupportedMediaType);

            using var ts = new StreamReader(Request.Form.Files[0].OpenReadStream());
            var fileContents = await ts.ReadToEndAsync();

            _service.LoadString(fileContents);

            return Ok(_service.GetAllStats());
        }
    }
}
