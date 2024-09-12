namespace Core.Specifications;

public class ProductSpecParams
{
    private const int MaxPageSize = 50;
    private int _pageIndex = 1;
    public int PageIndex
    {
        get => _pageIndex;
        // Clamp to prevent the index from being out of bounds or negative.
        set => _pageIndex = Math.Max(value, 1);
    }

    private int _pageSize = 6;

    public int PageSize
    {
        get => _pageSize;
        // Clamp to prevent the page size from being less than 1 or greater than the max
        set => _pageSize = Math.Clamp(value, 1, MaxPageSize); 
    }
    
    private List<string> _brands = [];
    public List<string> Brands
    {
        get => _brands;
        set
        {
            _brands = value.SelectMany(x => x.Split(',',
                StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }   
    
    private List<string> _types = [];

    public List<string> Types
    {
        get => _types;
        set
        {
            _types = value.SelectMany(x => x.Split(',',
                StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }
    
    public string? Sort { get; set; }

    private string? _search;
    public string? Search
    {
        get => _search ?? "";
        set => _search = value?.ToLower();
    }
}