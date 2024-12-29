using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace ToDo.Api.Setup
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDependencyInjection();
            //services.AddDbContext<MoviesRentalWriteContext>(options =>
            //{
            //    options.UseSqlServer(configuration.GetConnectionString("SqlConnection"), opt =>
            //    {
            //        opt.EnableRetryOnFailure();
            //    });
            //});

            return services;
        }
    }
}
