using MediatR;
using TokenManager.Application.Services.Mappers;
using TokenManager.Application.Services.Responses;
using TokenManager.Domain.Interfaces;

namespace TokenManager.Application.Services.Commands.Users
{
    public class LoginUserCommandHandler(IUserRepository userRepository) : IRequestHandler<LoginUserCommand, ResponseResult<TokenDetailsResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ResponseResult<TokenDetailsResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.LoginUser.ToDomain();
            var tokenDetailsResult = await _userRepository.LoginAsync(request.Tenant, user);
            if (tokenDetailsResult.IsSuccess)
            {
                return tokenDetailsResult.ToTokenResponse();
            }

            return ResponseResult<TokenDetailsResponse>.Failure(tokenDetailsResult.Error);
        }
    }
}
