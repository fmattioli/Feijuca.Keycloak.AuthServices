﻿using TokenManager.Domain.Entities;

namespace TokenManager.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<TokenDetails>> GetAccessTokenAsync(string tenant);
        Task<Result<TokenDetails>> LoginAsync(string tenant, User user);
        Task<Result<TokenDetails>> RefreshTokenAsync(string tenant, string refreshToken);
        Task<(bool result, string content)> CreateNewUserAsync(string tenant, User user);
        Task<Result<User>> GetUserAsync(string userName);
        Task<Result> ResetPasswordAsync(string userId, string password);
        Task<Result> SendEmailVerificationAsync(string userId);
    }
}
