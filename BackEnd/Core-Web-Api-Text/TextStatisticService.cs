using Core_Web_Api_Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace Core_Web_Api_Text
{
    public class TextStatisticService : ITextStatisticService
    {
        public string Text { get; private set; } = string.Empty;

        /// <summary>
        /// Loads a string into the service to be processed. Will throw an exception if the string is null
        /// </summary>
        /// <param name="textToLoad"> The string to process</param>
        public void LoadString(string? textToLoad)
        {
            ArgumentNullException.ThrowIfNull(textToLoad, nameof(textToLoad));
            Text = textToLoad;
        }

        /// <summary>
        /// Counts the number of sentences in a string
        ///  Sentences are split by a full stop
        /// </summary>
        /// <returns>the number of characters in the string</returns>
        public int GetSentenceCount()
        {
            return Text.Split('.', StringSplitOptions.RemoveEmptyEntries).Length;
        }

        /// <summary>
        /// Gets the number of lines in the string
        /// Lines are separated by a single new line.
        /// </summary>
        /// <returns>The number of lines in the string</returns>
        public int GetLineCount()
        {
            //Remove any multiple new lines just in case.
            var newText = Regex.Replace(Text, @"(\r\n){2,}", Environment.NewLine);
            int count = 1;

            for (int i = 0; i < newText.Length; i++)
            {
                if (newText[i] == '\n')
                    count++;
            }

            return count;
        }

        /// <summary>
        /// This counts the number of paragraphs  in the string
        ///  A Paragraph is text split by two new lines.
        /// </summary>
        /// <returns>The number of paragraphs in the string</returns>
        public int GetParagraphCount()
        {
            var paragraphs = Text.Split(new[] { Environment.NewLine + Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);
            return paragraphs.Length;
        }

        /// <summary>
        /// Gets the character count in the string
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The number of characters in the string</returns>
        public int GetCharacterCount()
        {
            StringBuilder sb = new(Text.Length);
            for (int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                switch (c)
                {
                    case '\r':
                    case '\n':
                    case '\t':
                    case ' ':
                        continue;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString().Length;
        }

        /// <summary>
        /// Gets the top ten words by usage in the string
        /// </summary>
        /// <returns>Up to ten word with their count of instances in the string</returns>
        public IReadOnlyDictionary<string, int> GetTopTenWords()
        {
            var words = Regex.Replace(Text, @"\W", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var result = words.Select(w => w.ToLower())
                            .GroupBy(w => w)
                            .Select(g => new { g.Key, Count = g.Count() })
                            .OrderByDescending(r => r.Count)
                            .ThenBy(r => r.Key)
                            .Take(10)
                            .ToDictionary(r => r.Key, r => r.Count);
            return result;
        }

        public ITextStatisticResult GetAllStats() => new TextStatisticResult()
        {
            CharacterCount = GetCharacterCount(),
            LineCount = GetLineCount(),
            ParagraphCount = GetParagraphCount(),
            SentenceCount = GetSentenceCount(),
            WordFrequency = GetTopTenWords()
        };
    }
}