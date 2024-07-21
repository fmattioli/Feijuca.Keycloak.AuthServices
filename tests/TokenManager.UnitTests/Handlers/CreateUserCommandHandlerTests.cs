using AutoFixture;
using FluentAssertions;
using Moq;
using TokenManager.Application.Services.Commands.Users;
using TokenManager.Domain.Entities;
using TokenManager.Domain.Errors;
using TokenManager.Domain.Interfaces;

namespace TokenManager.UnitTests.Handlers
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Fixture _autoFixture = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly CreateUserCommandHandler _createUserCommandHandler;

        public CreateUserCommandHandlerTests()
        {
            _createUserCommandHandler = new(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleWhenInformAValidUser_ShouldCreateANewUserAndReturnsTrue()
        {
            // Arrange
            var createUserCommand = _autoFixture.Create<CreateUserCommand>();
            var tokenDetails = _autoFixture.Create<TokenDetails>();
            var successResult = Result<TokenDetails>.Success(tokenDetails);

            _userRepositoryMock
                .Setup(x => x.GetAccessTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(successResult);

            (bool result, string content) createUserResult = (true, "Success message");

            _userRepositoryMock
                .Setup(x => x.CreateNewUserAsync(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(createUserResult);

            var user = _autoFixture.Create<User>();
            var userResult = Result<User>.Success(user);

            _userRepositoryMock
                .Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(userResult);
            
            _userRepositoryMock
                .Setup(x => x.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(userResult);

            //Act
            var resultHandle = await _createUserCommandHandler.Handle(createUserCommand, CancellationToken.None);

            //Assert
            resultHandle.IsSuccess
                .Should()
                .Be(true);

            _userRepositoryMock
                .Verify(x => x.GetAccessTokenAsync(It.IsAny<string>()), Times.Once);
            
            _userRepositoryMock
                .Verify(x => x.CreateNewUserAsync(It.IsAny<string>(), It.IsAny<User>()), Times.Once);
            
            _userRepositoryMock
                .Verify(x => x.GetUserAsync(It.IsAny<string>()), Times.Once);

            _userRepositoryMock
                .Verify(x => x.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task HandleWhenInformAInvalidConfig_ShouldNotCreateANewUserAndReturnsFalse()
        {
            // Arrange
            var createUserCommand = _autoFixture.Create<CreateUserCommand>();

            var errorMessage = _autoFixture.Create<string>();
            UserErrors.SetTechnicalMessage(errorMessage);
            var result = Result<TokenDetails>.Failure(UserErrors.TokenGenerationError);

            _userRepositoryMock
                .Setup(x => x.GetAccessTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(result);

            //Act
            var resultHandle = await _createUserCommandHandler.Handle(createUserCommand, CancellationToken.None);

            //Assert
            resultHandle.IsSuccess
                .Should()
                .Be(false);

            resultHandle.Error.Description
                .Should()
                .Contain(result.Error.Description);

            resultHandle.Error.Code
                .Should()
                .Contain(result.Error.Code);
        }

        [Fact]
        public async Task HandleWhenInformAInvalidUser_ShouldNotCreateANewUserAndReturnsFalse()
        {
            // Arrange
            var createUserCommand = _autoFixture.Create<CreateUserCommand>();

            var errorMessage = _autoFixture.Create<string>();
            UserErrors.SetTechnicalMessage(errorMessage);
            var successResult = Result<TokenDetails>.Success(_autoFixture.Create<TokenDetails>());
            

            _userRepositoryMock
                .Setup(x => x.GetAccessTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(successResult);

            _userRepositoryMock
                .Setup(x => x.CreateNewUserAsync(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync((false, "Some error"));
            //Act
            var resultHandle = await _createUserCommandHandler.Handle(createUserCommand, CancellationToken.None);

            //Assert
            resultHandle.IsSuccess
                .Should()
                .Be(false);

            resultHandle.Error.Description
                .Should()
                .Contain(UserErrors.WrongPasswordDefinition.Description);
        }
    }
}
