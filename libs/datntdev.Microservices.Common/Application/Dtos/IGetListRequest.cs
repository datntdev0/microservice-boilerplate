namespace datntdev.Microservices.Common.Application.Dtos
{
    public interface IPagedListRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public interface ISortedListRequest
    {
        public string? SortBy { get; set; }
        public int? SortDirection { get; set; } // 1 for ascending, -1 for descending
    }

    public interface ISearchListRequset
    {
        public string? Search { get; set; }
    }
}
