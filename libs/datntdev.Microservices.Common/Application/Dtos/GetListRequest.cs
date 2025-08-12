namespace datntdev.Microservices.Common.Application.Dtos
{
    public class PagedListRequest : IPagedListRequest
    {
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }

    public class SortedListRequest : PagedListRequest, ISortedListRequest
    {
        public string? SortBy { get; set; }
        public int? SortDirection { get; set; } = 1; // 1 for ascending, -1 for descending
    }

    public class SearchListRequest : SortedListRequest, ISearchListRequset
    {
        public string? Search { get; set; }
    }
}
