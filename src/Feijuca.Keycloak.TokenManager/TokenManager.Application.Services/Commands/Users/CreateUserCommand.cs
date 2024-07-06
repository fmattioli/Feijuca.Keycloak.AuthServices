using Contracts.Common;
using MediatR;
using TokenManager.Application.Services.Requests.User;
using TokenManager.Application.Services.Responses;

namespace TokenManager.Application.Services.Commands.Users
{
    public record CreateUserCommand(AddUserRequest AddUserRequest) : IRequest<Result<TokenResponse>>;
}
