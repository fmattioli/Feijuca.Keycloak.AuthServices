﻿using Contracts.Common;
using MediatR;
using TokenManager.Application.Services.Requests.User;
using TokenManager.Application.Services.Responses;

namespace TokenManager.Application.Services.Commands.Users
{
    public record LoginUserCommand(LoginUserRequest LoginUser) : IRequest<Result<TokenResponse>>;
}