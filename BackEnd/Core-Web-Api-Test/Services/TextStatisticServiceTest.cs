using Core_Web_Api_Text;

namespace Core_Web_Api_Test.Services
{
    public class TextStatisticServiceTest
    {
        [Fact]
        public void When_Loaded_With_Null_Then_Exception_Thrown()
        {
            // Arrange
            var service = new TextStatisticService();
            // Act

            Action act = ()=> service.LoadString(null);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void When_Loaded_With_Not_Null_Then_No_Exception_Thrown()
        {
            // Arrange
            var service = new TextStatisticService();
            Action act = ()=> service.LoadString("TEST");
            // Act

            var exception = Record.Exception(act);

            // Assert
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("a",1)]
        [InlineData("Test",4)]
        [InlineData("The Quick Brown Fox Jumps Over The Lazy Dog",35)]
        [InlineData("Foo",3)]

        public void When_character_count_called_then_correct_value_returned(string loadString, int charCount)
        {
            // Arrange
            var service = new TextStatisticService();
            // Act

             service.LoadString(loadString);

            // Assert
            Assert.Equal(service.GetCharacterCount(), charCount);
        }

        [Theory]
        [InlineData("one", new string[] {"one"}, new int[]{1})]
        [InlineData("one two one", new string[] {"one","two"}, new int[]{2,1})]
        [InlineData("one two three four five six seven eight nine ten", new string[] {"eight","five","four","nine","one","seven","six","ten","three","two"}, new int[]{1,1,1,1,1,1,1,1,1,1})]
        [InlineData("one tHree three Four four fOur nIne nine niNe Ten", new string[] {"four","nine","three","one","ten"}, new int[]{3,3,2,1,1})]

        public void When_word_rank_called_then_correct_value_returned(string loadString,string[] expectedWords, int[] expectedCounts)
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

    }
}