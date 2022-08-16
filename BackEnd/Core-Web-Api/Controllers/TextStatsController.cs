using System.Text;
using Core_Web_Api_Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextStatsController : ControllerBase
    {
        /// <summary>
        /// Gets statistical details from a string provided
        /// </summary>
        /// <param name="requestedText">The text to be analyzed.</param>
        /// <returns>A set of statistics</returns>
        [HttpPost(Name = "GetTextStats")]
        public ActionResult GetTextStats([FromBody]string requestedText)
        {
            var service = new TextStatisticService();
            service.LoadString(requestedText);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Character count: " + service.GetCharacterCount());
            sb.AppendLine("Line count: " + service.GetLineCount());
            sb.AppendLine("Paragraph count: " + service.GetParagraphCount());
            sb.AppendLine("Sentence count: " + service.GetSentenceCount());
            // sb.AppendLine("Top Ten Words:");
            //var topTenWords = service.GetTopTenWords();
            //foreach (var entry in topTenWords)
            //{
            //    sb.AppendLine(entry.Key + " : " + entry.Value);
            //}

            return Ok(sb.ToString());
        }

        /// <summary>
        /// Gets statistical details from a file provided
        /// </summary>
        /// <returns>A set of statistics</returns>
        /// <exception cref="NotImplementedException"></exception>
        
        [HttpPost(Name = "GetTextFileStats")]
        public ActionResult GetTextFileStats()
        {
            throw new NotImplementedException();

        }
        
    }
}
