
using WebApiAcademy.DTOs;

namespace WebApiAcademy.Utils
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> GetPaged<T>(this IQueryable<T> queryable, Pagination paginationDTO)
        {
            return queryable
                .Skip((paginationDTO.Page - 1) * paginationDTO.PerPage)
                .Take(paginationDTO.PerPage);
        }

    }
}
