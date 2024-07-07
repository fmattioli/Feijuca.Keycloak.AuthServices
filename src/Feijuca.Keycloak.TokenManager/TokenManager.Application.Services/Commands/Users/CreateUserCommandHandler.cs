using Contracts.Common;
using MediatR;
using TokenManager.Application.Services.Mappers;
using TokenManager.Domain.Interfaces;

namespace TokenManager.Application.Services.Commands.Users
{
    public class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var resultUserCreated = await _userRepository.CreateUserAsync(request.AddUserRequest.ToDomain());
            if (resultUserCreated.IsSuccess)
            {
                return Result.Success();
            }

            return Result.Failure(resultUserCreated.Error);
        }
    }
}
