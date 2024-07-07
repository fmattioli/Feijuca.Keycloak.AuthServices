using Contracts.Common;
using TokenManager.Domain.Entities;

namespace TokenManager.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<TokenDetails>> GetAccessTokenAsync(string tenant);
        Task<Result> CreateUserAsync(string tenant, User user);
        Task<Result<TokenDetails>> LoginAsync(User user);
    }
}
