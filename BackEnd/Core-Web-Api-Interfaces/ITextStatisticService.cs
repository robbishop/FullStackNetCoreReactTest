namespace Core_Web_Api_Interfaces
{
    public interface ITextStatisticService
    {
        string Text { get; }

        ITextStatisticResult GetAllStats();
        int GetCharacterCount();
        int GetLineCount();
        int GetParagraphCount();
        int GetSentenceCount();
        IReadOnlyDictionary<string, int> GetTopTenWords();
        void LoadString(string? textToLoad);
    }
}