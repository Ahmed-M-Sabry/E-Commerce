using ECommerce.Application.IServices;
using ECommerce.Domain.IRepositories;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Add your infrastructure dependencies here, e.g. DbContext, Repositories, etc.
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            // Services
            services.AddTransient<IIdentityServies, IdentityServies>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IUserContextService, UserContextService>();
            services.AddTransient<IEmailService, EmailService>();





            return services;
        }
    }
}
