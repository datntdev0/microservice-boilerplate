namespace datntdev.Microservices.Common.Application.Dtos
{
    public class GetListResponse<TListDto> where TListDto : class
    {
        public List<TListDto> Items { get; set; } = [];
        public int TotalCount { get; set; } = 0;
    }
}
