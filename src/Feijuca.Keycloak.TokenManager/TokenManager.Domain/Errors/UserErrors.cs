using Contracts.Common;

namespace TokenManager.Domain.Errors
{
    public static class UserErrors
    {
        public static readonly Error TokenGenerationError = new("User.TokenGeneration", "An error occured while trying receive informations about realm");
    }
}
