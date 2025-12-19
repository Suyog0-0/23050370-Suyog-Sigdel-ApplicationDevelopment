namespace _23050370_Suyog_Sigdel.Services;

public class SearchService
{
    public string SearchTerm { get; private set; } = string.Empty;
    public event Action? OnSearchChanged;

    public void SetSearchTerm(string term)
    {
        SearchTerm = term;
        OnSearchChanged?.Invoke();
    }

    public void ClearSearch()
    {
        SearchTerm = string.Empty;
        OnSearchChanged?.Invoke();
    }
}