using Contracts.Common;
using MediatR;
using TokenManager.Application.Services.Responses;

namespace TokenManager.Application.Services.Commands.Users
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<TokenResponse>>
    {
        public Task<Result<TokenResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
