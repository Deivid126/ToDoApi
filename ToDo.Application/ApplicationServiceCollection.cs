using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ToDo.Application
{
    public static class ApplicationServiceCollection
    {
        public static void AddDependcyApplication(this IServiceCollection services) 
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Scoped);
        }
    }
}