using TokenManager.Application.Services.Requests.User;
using TokenManager.Application.Services.Responses;
using TokenManager.Domain.Entities;

namespace TokenManager.Application.Services.Mappers
{
    public static class UserMapper
    {
        public static User ToDomain(this AddUserRequest userRequest, string tenant)
        {
            var attributes = userRequest.Attributes!.ToDomain(tenant);
            return new User(userRequest.Username!, userRequest.Password, userRequest.Email!, userRequest.FirstName!, userRequest.LastName!, attributes);
        }

        public static Attributes ToDomain(this AttributesRequest attributes, string tenant)
        {
            return new Attributes(attributes.ZoneInfo, attributes.Birthdate, attributes.PhoneNumber, attributes.Gender, attributes.Fullname, tenant, attributes.Picture);
        }

        public static User ToDomain(this LoginUserRequest loginUserRequest, string tenant)
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
                RefreshExpiresIn = tokenDetails.Refresh_Expires_In
            };
        }
    }
}
