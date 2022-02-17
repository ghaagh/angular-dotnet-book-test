namespace Book.Application.Domain.Helper
{
    public interface IQueryHelper<T> where T : class
    {
        IQueryable<T> ApplySearch(IQueryable<T> entities, string searchValue, string searchFields);
        IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString);
        Task<Paged<T>> ApplyPaginationAsync(IQueryable<T> entities, int currentPage, int pageSize);
    }
}
