using ECommerce.Application.IServices;
using ECommerce.Application.IServices.IProductServ;
using ECommerce.Application.IServices.ProductServ;
using ECommerce.Domain.IRepositories;
using ECommerce.Domain.IRepositories.IOrderRepo;
using ECommerce.Domain.IRepositories.ProductsRepo;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Repositories.OrderRepo;
using ECommerce.Infrastructure.Repositories.ProductsRepo;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Services.ProductServ;
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
            services.AddScoped<IIdentityServies, IdentityServies>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService,ProductService>();

            // Repo
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<ICustomerBasketRepository, CustomerBasketRepository>();


            // IUnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
