namespace Book.Application.Domain.Repository
{
    public interface ISaver
    {
        Task<int> SaveAsync();
    }
}
