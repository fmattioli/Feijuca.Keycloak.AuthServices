using Contracts.Common;
using MediatR;
using TokenManager.Application.Services.Mappers;
using TokenManager.Application.Services.Responses;
using TokenManager.Domain.Interfaces;

namespace TokenManager.Application.Services.Commands.Users
{
    public class LoginUserCommandHandler(IUserRepository userRepository) : IRequestHandler<LoginUserCommand, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<TokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.LoginUser.ToDomain();
            var tokenDetailsResult = await _userRepository.LoginAsync(request.Tenant, user);
            if (tokenDetailsResult.IsSuccess)
            {
                return Result<TokenResponse>.Success(tokenDetailsResult.Value.ToTokenResponse());
            }

            return Result<TokenResponse>.Failure(tokenDetailsResult.Error);
        }
    }
}
