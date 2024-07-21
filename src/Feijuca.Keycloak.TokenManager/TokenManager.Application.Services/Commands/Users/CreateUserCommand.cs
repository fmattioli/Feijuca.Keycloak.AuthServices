using MediatR;
using TokenManager.Application.Services.Requests.User;
using TokenManager.Domain.Entities;

namespace TokenManager.Application.Services.Commands.Users
{
    public record CreateUserCommand(string Tenant, AddUserRequest AddUserRequest) : IRequest<Result>;
}
