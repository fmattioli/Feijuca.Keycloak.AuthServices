using TokenManager.Application.Services.Requests.User;
using TokenManager.Application.Services.Responses;
using TokenManager.Domain.Entities;

namespace TokenManager.Application.Services.Mappers
{
    public static class UserMapper
    {
        public static User ToDomain(this AddUserRequest userRequest)
        {            
            return new User(userRequest.Username!, userRequest.Password, userRequest.Email!, userRequest.FirstName!, userRequest.LastName!, userRequest.Attributes);
        }


        public static User ToDomain(this LoginUserRequest loginUserRequest)
        {
            return new User(loginUserRequest.Username, loginUserRequest.Password);
        }

        public static TokenResponse ToTokenResponse(this TokenDetails tokenDetails)
        {
            return new TokenResponse
            {
                AccessToken = tokenDetails.Access_Token,
                ExpiresIn = tokenDetails.Expires_In,
                RefreshToken = tokenDetails.Refresh_Token,
                RefreshExpiresIn = tokenDetails.Refresh_Expires_In,
                TokenType = tokenDetails.Token_Type,
                Scope = tokenDetails.Scope
            };
        }
    }
}
