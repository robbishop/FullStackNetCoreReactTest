namespace Core_Web_Api_Text
{
    public class TextStatisticResult
    {
        public int CharacterCount { get; init; }
        public int LineCount { get; init; }
        public int ParagraphCount { get; init; }
        public int SentenceCount { get; init; }
        public IReadOnlyDictionary<string, int> WordFrequency { get; init; } = new Dictionary<string, int>();

        public override string ToString()
        {
            return $"Character count: {CharacterCount}\r\nLine count: {LineCount}\r\nParagraph count: {ParagraphCount}\r\nSentence count: {SentenceCount}\r\n";
        }
    }
}