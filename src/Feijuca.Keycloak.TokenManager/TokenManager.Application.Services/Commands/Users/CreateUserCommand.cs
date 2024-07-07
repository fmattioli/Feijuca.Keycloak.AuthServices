﻿using Contracts.Common;
using MediatR;
using TokenManager.Application.Services.Requests.User;

namespace TokenManager.Application.Services.Commands.Users
{
    public record CreateUserCommand(AddUserRequest AddUserRequest) : IRequest<Result>;
}
