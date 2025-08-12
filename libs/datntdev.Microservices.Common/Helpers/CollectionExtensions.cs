using datntdev.Microservices.Common.Application.Dtos;
using datntdev.Microservices.Common.Models;
using System.Linq.Expressions;

namespace datntdev.Microservices.Common.Helpers
{
    public static class CollectionExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IQueryable<T> Search<T>(this IQueryable<T> source, ISearchListRequset search,
            Expression<Func<T, bool>> predicate)
        {
            return string.IsNullOrWhiteSpace(search.Search) ? source : source.Where(predicate);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, ISortedListRequest sorting)
        {
            if (string.IsNullOrWhiteSpace(sorting.SortBy)) 
                return source.OrderBy(x => "Id");

            return sorting.SortDirection == -1
                ? source.OrderByDescending(x => sorting.SortBy)
                : source.OrderBy(x => sorting.SortBy);
        }

        public static IQueryable<T> Paging<T>(this IQueryable<T> source, IPagedListRequest paging)
        {
            return source.Skip(paging.PageNumber * paging.PageSize).Take(paging.PageSize);
        }
    }
}
