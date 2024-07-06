using Contracts.Common;
using TokenManager.Domain.Entities;

namespace TokenManager.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<string>> GetAccessToken(GetTokenDetails getTokenDetails);
        Task<Result> CreateUser(User user);
    }
}
