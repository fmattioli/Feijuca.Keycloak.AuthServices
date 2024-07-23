using MediatR;
using TokenManager.Application.Services.Responses;

namespace TokenManager.Application.Services.Commands.Users
{
    public record RefreshTokenCommand(string Tenant, string RefreshToken) : IRequest<ResponseResult<TokenDetailsResponse>>;
}
