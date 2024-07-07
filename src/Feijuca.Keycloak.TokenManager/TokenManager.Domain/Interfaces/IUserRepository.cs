using Contracts.Common;
using TokenManager.Domain.Entities;

namespace TokenManager.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<TokenDetails>> GetAccessTokenAsync(string tenant);
        Task<Result> DoNewUserCreationActions(string tenant, User user);
        Task<HttpResponseMessage> CreateNewUserAsync(User user);
        Task<Result<User>> GetUserAsync(string userName);
        Task<Result<TokenDetails>> LoginAsync(User user);
    }
}
