using Microsoft.AspNetCore.Mvc.Filters;

namespace Feijuca.Keycloak.MultiTenancy.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredScopeAttribute(string scope) : Attribute, IAuthorizationFilter
    {
        private readonly string _scope = scope;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            var scopeClaim = user.FindFirst("scope");
            if (scopeClaim == null || !scopeClaim.Value.Split(' ').Contains(_scope))
            {
                throw new UnauthorizedAccessException($"Scope {_scope} not attribuited on user {user!.Identity!.Name}.");
            }
        }
    }
}
