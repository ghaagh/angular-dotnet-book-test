using Book.Application.Infrastructure;

namespace Book.Application.Domain.Repository
{
    public interface ISaver
    {
        Task<int> SaveAsync();
    }
}
