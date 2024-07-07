using Contracts.Common;
using TokenManager.Domain.Entities;

namespace TokenManager.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<TokenDetails>> GetAccessTokenAsync();
        Task<Result> CreateUserAsync(CreateUser user);
        Task<Result<TokenDetails>> LoginAsync(LoginUser user);
    }
}
