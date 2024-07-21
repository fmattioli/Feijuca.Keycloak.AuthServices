using MediatR;
using TokenManager.Application.Services.Requests.User;
using TokenManager.Application.Services.Responses;
using TokenManager.Domain.Entities;

namespace TokenManager.Application.Services.Commands.Users
{
    public record LoginUserCommand(string Tenant, LoginUserRequest LoginUser) : IRequest<Result<TokenResponse>>;
}
