namespace Core_Web_Api_Interfaces
{
    public interface ITextStatisticResult
    {
        int CharacterCount { get; init; }
        int LineCount { get; init; }
        int ParagraphCount { get; init; }
        int SentenceCount { get; init; }
        IReadOnlyDictionary<string, int> WordFrequency { get; init; }

        string ToString();
    }
}