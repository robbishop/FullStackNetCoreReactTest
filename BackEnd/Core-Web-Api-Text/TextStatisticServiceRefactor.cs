using Core_Web_Api_Interfaces;

namespace Core_Web_Api_Text
{
    public class TextStatisticServiceRefactor : ITextStatisticService
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
            if (Text.Length == 0) return 0;

            int fullStopCount = 0;
            foreach (char c in Text)
            {
                if (c == '.')
                {
                    fullStopCount++;
                }
            }

            if (!Char.IsPunctuation(Text[^1]))
                fullStopCount++;

            return fullStopCount;
        }

        /// <summary>
        /// Gets the number of lines in the string
        /// Lines are separated by a single new line.
        /// </summary>
        /// <returns>The number of lines in the string</returns>
        public int GetLineCount()
        {
            int count = 1;

            if (Text.Length == 0) return count;

            if (Text[0] == '\n')
            {
                count++;
            }

            char previousChar = Text[0];
            for (int i = 1; i < Text.Length; i++)
            {
                if (Text[i] == '\n' && previousChar != '\n')
                {
                    count++;
                }

                if (Text[i] != '\r')
                    previousChar = Text[i];
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
            int characterCount = 0;

            foreach (char c in Text)
            {
                if (!char.IsWhiteSpace(c))
                {
                    characterCount++;
                }
            }
            return characterCount;
        }

        /// <summary>
        /// Gets the top ten words by usage in the string
        /// </summary>
        /// <returns>Up to ten word with their count of instances in the string</returns>
        public IReadOnlyDictionary<string, int> GetTopTenWords()
        {
            var result = GetWords().Select(w => w.ToLower())
                            .GroupBy(w => w)
                            .Select(g => new { g.Key, Count = g.Count() })
                            .OrderByDescending(r => r.Count)
                            .ThenBy(r => r.Key)
                            .Take(10)
                            .ToDictionary(r => r.Key, r => r.Count);
            return result;
        }

        private List<string> GetWords()
        {
            var words = new List<string>();
            var currentWord = new List<char>();

            void BuildWord()
            {
                if (currentWord.Count > 0)
                {
                    var newWord = new string(currentWord.ToArray());
                    if (!string.IsNullOrEmpty(newWord))
                    {
                        words.Add(newWord);
                    }
                    currentWord.Clear();
                }
            }

            for (int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                if (c == ' ')
                {
                    BuildWord();
                    continue;
                }

                if (char.IsLetterOrDigit(c))
                    currentWord.Add(c);
            }
            BuildWord();
            return words;
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