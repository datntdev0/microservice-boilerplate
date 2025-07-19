namespace datntdev.Microservices.Common.Application.Dtos
{
    public class PagedListRequest : IPagedListRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class SortedListRequest : PagedListRequest, ISortedListRequest
    {
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }

    public class SearchListRequest : SortedListRequest, ISearchListRequset
    {
        public string? Search { get; set; }
    }
}
