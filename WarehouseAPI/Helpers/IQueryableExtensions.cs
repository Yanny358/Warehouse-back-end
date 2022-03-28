using WarehouseAPI.DTOs;

namespace WarehouseAPI.Helpers;

public static class IQueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDto)
    {
        return queryable.Skip((paginationDto.Page - 1) * paginationDto.RecordsPerPage)
            .Take(paginationDto.RecordsPerPage);
    }
}