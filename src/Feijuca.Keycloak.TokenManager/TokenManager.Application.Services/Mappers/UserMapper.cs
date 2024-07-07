using TokenManager.Application.Services.Requests.User;
using TokenManager.Domain.Entities;

namespace TokenManager.Application.Services.Mappers
{
    public static class UserMapper
    {
        public static CreateUser ToDomain(this AddUserRequest userRequest)
        {
            var attributes = userRequest.Attributes!.ToDomain();
            return new CreateUser(userRequest.Username, userRequest.Email, userRequest.FirstName, userRequest.LastName, attributes);
        }

        public static Domain.Entities.Attributes ToDomain(this Requests.User.Attributes attributes)
        {
            return new Domain.Entities.Attributes(attributes.ZoneInfo, attributes.Birthdate, attributes.PhoneNumber, attributes.Gender, attributes.Fullname, attributes.Tenant, attributes.Picture);
        }
    }
}
