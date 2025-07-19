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
        public string? SortDirection { get; set; }
    }

    public interface ISearchListRequset
    {
        public string? Search { get; set; }
    }
}
