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
            AddTenantToRequest(request);
            var accessTokenResult = await _userRepository.GetAccessTokenAsync(request.Tenant);
            if (accessTokenResult.IsSuccess)
            {
                var user = request.AddUserRequest.ToDomain();

                var (IsSuccessStatusCode, contentRequest) = await _userRepository.CreateNewUserAsync(request.Tenant, user);
                if (IsSuccessStatusCode)
                {
                    await SetUserPasswordAsync(user);
                    return Result.Success();
                }

                UserErrors.SetTechnicalMessage(contentRequest);
                return Result.Failure(UserErrors.WrongPasswordDefinition);
            }

            return Result.Failure(UserErrors.TokenGenerationError);
        }

        private static void AddTenantToRequest(CreateUserCommand request)
        {
            request.AddUserRequest.Attributes.Add("tenant", [request.Tenant]);
        }

        private async Task SetUserPasswordAsync(User user)
        {
            var keycloakUser = await _userRepository.GetUserAsync(user.Username!);
            await _userRepository.ResetPasswordAsync(keycloakUser.Value.Id!, user.Password!);
        }
    }
}
