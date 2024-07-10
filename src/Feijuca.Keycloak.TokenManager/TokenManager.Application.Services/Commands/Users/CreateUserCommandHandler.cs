using Contracts.Common;
using MediatR;
using TokenManager.Application.Services.Mappers;
using TokenManager.Domain.Entities;
using TokenManager.Domain.Errors;
using TokenManager.Domain.Interfaces;

namespace TokenManager.Application.Services.Commands.Users
{
    public class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var accessTokenResult = await _userRepository.GetAccessTokenAsync(request.Tenant);
            if (accessTokenResult.IsSuccess)
            {
                var user = request.AddUserRequest.ToDomain();
                var(IsSuccessStatusCode, contentRequest) = await _userRepository.CreateNewUserAsync(user);
                if (IsSuccessStatusCode)
                {
                    await SetUserPasswordAsync(user);
                    return Result.Success();
                }

                UserErrors.SetTechnicalMessage(contentRequest);
                return Result.Failure(UserErrors.TokenGenerationError);
            }

            return Result.Failure(UserErrors.InvalidUserNameOrPasswordError);
        }

        private async Task SetUserPasswordAsync(User user)
        {
            var keycloakUser = await _userRepository.GetUserAsync(user.Username!);
            await _userRepository.ResetPasswordAsync(keycloakUser.Value.Id!, user.Password!);
        }
    }
}
