using Core_Web_Api_Interfaces;
using Core_Web_Api_Text;

namespace Core_Web_Api_Test.Services
{
    public class TextStatisticServiceTestBase
    {
        [Fact]
        public void When_Loaded_With_Null_Then_Exception_Thrown()
        {
            // Arrange
            var service = new TextStatisticService();

            // Act
            void act() => service.LoadString(null);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void When_Loaded_With_Not_Null_Then_No_Exception_Thrown()
        {
            // Arrange
            var service = new TextStatisticService();

            // Act
            void act() => service.LoadString("TEST");
            var exception = Record.Exception(act);

            // Assert
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("one", 1)]
        [InlineData("one. two", 2)]
        [InlineData("one. two.", 2)]
        public void When_sentance_count_called_then_correct_value_returned(string loadString, int sentanceCount)
        {
            // Arrange
            var service = new TextStatisticService();

            // Act
            service.LoadString(loadString);

            // Assert
            Assert.Equal(sentanceCount, service.GetSentenceCount());
        }

        [Theory]
        [InlineData("", 1)]
        [InlineData("one", 1)]
        [InlineData("one\r\n", 2)]
        [InlineData("one\r\n\r\n", 2)]
        [InlineData("ONE\r\nTWO\r\nONE\r\n\r\nTWO\r\n\r\n\r\nONE\r\nTWO\r\n", 7)]

        public void When_line_count_called_then_correct_value_returned(string loadString, int lineCount)
        {
            // Arrange
            var service = new TextStatisticService();

            // Act
            service.LoadString(loadString);

            // Assert
            Assert.Equal(lineCount, service.GetLineCount());
        }

        [Theory]
        [InlineData("a", 1)]
        [InlineData("Test", 4)]
        [InlineData("The Quick Brown Fox Jumps Over The Lazy Dog", 35)]
        [InlineData("Foo", 3)]

        public void When_character_count_called_then_correct_value_returned(string loadString, int charCount)
        {
            // Arrange
            var service = new TextStatisticService();

            // Act
            service.LoadString(loadString);

            // Assert
            Assert.Equal(charCount, service.GetCharacterCount());
        }

        [Theory]
        [InlineData("one", new string[] { "one" }, new int[] { 1 })]
        [InlineData("one two one", new string[] { "one", "two" }, new int[] { 2, 1 })]
        [InlineData(" one. two   one two,   one! two?   one (one) <two> {one} ", new string[] { "one", "two" }, new int[] { 6, 4 })]
        [InlineData("one two three four five six seven eight nine ten", new string[] { "eight", "five", "four", "nine", "one", "seven", "six", "ten", "three", "two" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 })]
        [InlineData("one tHree three Four four fOur nIne nine niNe Ten", new string[] { "four", "nine", "three", "one", "ten" }, new int[] { 3, 3, 2, 1, 1 })]

        public void When_word_rank_called_then_correct_value_returned(string loadString, string[] expectedWords, int[] expectedCounts)
        {
            // Arrange
            var service = new TextStatisticService();

            // Act
            service.LoadString(loadString);
            var topTenWords = service.GetTopTenWords();

            // Assert
            Assert.True(expectedWords.SequenceEqual(topTenWords.Keys));
            Assert.True(expectedCounts.SequenceEqual(topTenWords.Values));
        }

        [Fact]
        public void When_get_all_stats_called_all_values_should_be_in_result()
        {
            var service = new TextStatisticService();
            service.LoadString("one Two one");

            var result = service.GetAllStats();

            var expected = new TextStatisticResult
            {
                CharacterCount = 9,
                LineCount = 1,
                ParagraphCount = 1,
                SentenceCount = 1,
                WordFrequency = new Dictionary<string, int> { { "one", 2 }, { "two", 1 } }
            };

            Assert.True(result.IsEqualTo(expected));
        }
    }

    public static class TextStatisticResultExtensions
    {
        public static bool IsEqualTo(this ITextStatisticResult result, TextStatisticResult other) => result.CharacterCount == other.CharacterCount &&
                result.LineCount == other.LineCount &&
                result.ParagraphCount == other.ParagraphCount &&
                result.SentenceCount == other.SentenceCount &&
                (result.WordFrequency?.Keys.SequenceEqual(other.WordFrequency?.Keys ?? Enumerable.Empty<string>()) ?? false) &&
                (result.WordFrequency?.Values.SequenceEqual(other.WordFrequency?.Values ?? Enumerable.Empty<int>()) ?? false);
    }
}