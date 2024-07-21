using AutoFixture;
using FluentAssertions;
using Moq;
using TokenManager.Application.Services.Commands.Users;
using TokenManager.Domain.Entities;
using TokenManager.Domain.Interfaces;

namespace TokenManager.UnitTests.Handlers
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Fixture _autoFixture = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly LoginUserCommandHandler _loginUserCommandHandler;

        public LoginUserCommandHandlerTests()
        {
            _loginUserCommandHandler = new(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleWhenInformAValidUser_ShouldLoggedAndGenerateNewTokenDetails()
        {
            // Arrange
            var loginUserCommand = _autoFixture.Create<LoginUserCommand>();

            var tokenDetails = _autoFixture.Create<TokenDetails>();
            var successResult = Result<TokenDetails>.Success(tokenDetails);

            _userRepositoryMock
                .Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(successResult);

            //Act
            var resultHandle = await _loginUserCommandHandler.Handle(loginUserCommand, CancellationToken.None);

            //Assert
            resultHandle.IsSuccess
                .Should()
                .Be(true);

            resultHandle.Value.AccessToken
                .Should()
                .Be(tokenDetails.Access_Token);

            resultHandle.Value.ExpiresIn
                .Should()
                .Be(tokenDetails.Expires_In);

            resultHandle.Value.RefreshExpiresIn
                .Should()
                .Be(tokenDetails.Refresh_Expires_In);

            resultHandle.Value.RefreshToken
                .Should()
                .Be(tokenDetails.Refresh_Token);

            resultHandle.Value.TokenType
                .Should()
                .Be(tokenDetails.Token_Type);
        }

        [Fact]
        public async Task HandleWhenInformAInvalidUser_ShouldNotBeLoggedAndShouldReturnsAnError()
        {
            // Arrange
            var loginUserCommand = _autoFixture.Create<LoginUserCommand>();

            var error = _autoFixture.Create<Error>();
            var failureResult = Result<TokenDetails>.Failure(error);

            _userRepositoryMock
                .Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(failureResult);

            //Act
            var resultHandle = await _loginUserCommandHandler.Handle(loginUserCommand, CancellationToken.None);

            //Assert
            resultHandle.IsSuccess
                .Should()
                .Be(false);

            resultHandle.Error.Description
                .Should()
                .Contain(error.Description);

            resultHandle.Error.Code
                .Should()
                .Contain(error.Code);
        }
    }
}
