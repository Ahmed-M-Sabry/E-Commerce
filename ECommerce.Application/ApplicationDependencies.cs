using AutoMapper;
using ECommerce.Application.Features.AuthenticationFeatures.LoginUser.Command.Model;
using ECommerce.Application.Features.AuthenticationFeatures.LoginUser.Command.Validator;
using ECommerce.Application.Features.AuthenticationFeatures.RegisterBuyer.Command.Model;
using ECommerce.Application.Features.AuthenticationFeatures.RegisterBuyer.Command.Validation;
using ECommerce.Application.Features.AuthenticationFeatures.RegisterSeller.Command.Model;
using ECommerce.Application.Features.AuthenticationFeatures.RegisterSeller.Command.Validator;
using ECommerce.Application.PipelineBehaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerce.Application
{
    public static class ApplicationDependencies
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {

            //services.AddMediatR(c => c.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AdminAuthorizationBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(SellerAuthorizationBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PublicAuthorizationBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            services.AddScoped<IValidator<AddBuyerUserCommand>, AddBuyerUserValidator>();
            services.AddScoped<IValidator<AddSellerUserCommand>, AddSellerUserValidator>();
            services.AddScoped<IValidator<UserLogInCommand>, UserLogInValidator>();

            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
















            return services;
        }
    }
}
