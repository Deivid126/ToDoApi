using ToDo.Application;
using ToDo.Infrastructure;

namespace ToDo.Api.Setup
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDependecyInfrastructure(configuration);
            services.AddDependcyApplication();
            return services;
        }
    }
}
