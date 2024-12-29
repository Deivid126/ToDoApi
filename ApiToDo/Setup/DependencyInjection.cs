using ToDo.Application;
using ToDo.Infrastructure;

namespace ToDo.Api.Setup
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddDependecyInfrastructure();
            services.AddDependcyApplication();
            return services;
        }
    }
}
