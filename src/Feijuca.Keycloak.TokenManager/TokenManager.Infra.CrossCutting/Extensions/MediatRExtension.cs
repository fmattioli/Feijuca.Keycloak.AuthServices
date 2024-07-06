using Microsoft.Extensions.DependencyInjection;
using TokenManager.Application.Services.Commands.Users;

namespace TokenManager.Infra.CrossCutting.Extensions
{
    public static class MediatRExtension
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(
                    x => x.RegisterServicesFromAssemblies(typeof(CreateUserCommand).Assembly));

            return services;
        }
    }
}
