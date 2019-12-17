using Microsoft.Extensions.DependencyInjection;
using CentralDeErros.Application.ApplicationServices;
using CentralDeErros.Application.Interface;
using CentralDeErros.Data.Repositories;
using CentralDeErros.Domain.Interfaces.Repositories;
using CentralDeErros.Domain.Interfaces.Services;
using CentralDeErros.Domain.Services;

namespace CentralDeErros.CrossCutting.IoC
{
    public class RegisterIoC
    {
        public static void Register(IServiceCollection serviceCollection)
        {
            // AppServices
            serviceCollection.AddScoped<ILogAppService, LogAppService>();
            serviceCollection.AddScoped<IUserAppService, UserAppService>();
            

            // Services
            serviceCollection.AddScoped<ILogService, LogService>();
            serviceCollection.AddScoped<IUserService, UserService>();


            // Repositories
            serviceCollection.AddScoped<ILogRepository, LogRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();

        }
    }
}
