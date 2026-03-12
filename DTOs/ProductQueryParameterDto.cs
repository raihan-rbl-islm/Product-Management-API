namespace ProductManagementApi.DTOs;

public record ProductQueryParameterDto
{
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public string? Search { get; init; }
    public int? CategoryId { get; init; }
    public string? SortBy { get; init; }
    public bool? SortDescending { get; init; } = false;
    private int _pageNumber = 1;
    public int PageNumber
    {
        get => _pageNumber;
        init => _pageNumber = (value < 1) ? 1 : value;
    }
    private int _pageSize = 10;
    private const int MaxPageSize = 100;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = (value < 1) ? 10 : (value > MaxPageSize) ? MaxPageSize : value;
    }
}