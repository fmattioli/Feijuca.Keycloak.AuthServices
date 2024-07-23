using MediatR;
using TokenManager.Application.Services.Mappers;
using TokenManager.Application.Services.Responses;
using TokenManager.Domain.Interfaces;

namespace TokenManager.Application.Services.Commands.Users
{
    public class RefreshTokenCommandHandler(IUserRepository userRepository) : IRequestHandler<RefreshTokenCommand, ResponseResult<TokenDetailsResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ResponseResult<TokenDetailsResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokeDetails = await _userRepository.RefreshTokenAsync(request.Tenant, request.RefreshToken);
            return tokeDetails.ToTokenResponse();
        }
    }
}
