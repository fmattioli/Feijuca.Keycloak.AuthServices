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

        public static ResponseResult<TokenDetailsResponse> ToTokenResponse(this Result<TokenDetails> tokenDetails)
        {
            var tokenDetailsResponse = new TokenDetailsResponse
            {
                AccessToken = tokenDetails.Value.Access_Token,
                ExpiresIn = tokenDetails.Value.Expires_In,
                RefreshToken = tokenDetails.Value.Refresh_Token,
                RefreshExpiresIn = tokenDetails.Value.Refresh_Expires_In,
                TokenType = tokenDetails.Value.Token_Type,
                Scopes = tokenDetails.Value.Scopes
            };

            return ResponseResult<TokenDetailsResponse>.Success(tokenDetailsResponse);
        }
    }
}
