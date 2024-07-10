using Contracts.Common;
using MediatR;
using TokenManager.Application.Services.Requests.User;

namespace TokenManager.Application.Services.Commands.Users
{
    public record CreateUserCommand(string Tenant, AddUserRequest AddUserRequest) : IRequest<Result>;
}
