using datntdev.Microservices.Common.Application.Dtos;

namespace datntdev.Microservices.Common.Application
{
    public interface IApplicationService
    {
    }

    public interface IApplicationService<TKey, TDto, TListDto, TListRequest> : IApplicationService
        where TKey : IEquatable<TKey>
        where TDto : class
        where TListDto : class
        where TListRequest : class
    {
        Task<GetListResponse<TListDto>> GetListAsync(TListRequest input);
        Task<TDto> GetAsync(TKey id);
    }

    public interface IApplicationService<TKey, TDto, TListDto, TListRequest, TCreateRequest, TUpdateRequest> 
        : IApplicationService<TKey, TDto, TListDto, TListRequest>
        where TKey : IEquatable<TKey>
        where TDto : class
        where TListDto : class
        where TListRequest : class
        where TCreateRequest : class
        where TUpdateRequest : class
    {
        Task<TDto> CreateAsync(TCreateRequest input);
        Task<TDto> UpdateAsync(TUpdateRequest input);
        Task DeleteAsync(TKey id);
    }
}
