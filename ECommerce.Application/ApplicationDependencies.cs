using AutoMapper;
using ECommerce.Application.Features.AuthenticationFeatures.LoginAndTokens.LoginUser.Command.Model;
using ECommerce.Application.Features.AuthenticationFeatures.LoginAndTokens.LoginUser.Command.Validator;
using ECommerce.Application.Features.AuthenticationFeatures.Password.RestPassword.Command;
using ECommerce.Application.Features.AuthenticationFeatures.Password.RestPassword.Validator;
using ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterBuyer.Command.Model;
using ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterBuyer.Command.Validation;
using ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterSeller.Command.Model;
using ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterSeller.Command.Validator;
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

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AdminAuthorizationBehavior<,>));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(SellerAuthorizationBehavior<,>));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(BuyerAuthorizationBehavior<,>));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PublicAuthorizationBehavior<,>));

            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            services.AddScoped<IValidator<AddBuyerUserCommand>, AddBuyerUserValidator>();
            services.AddScoped<IValidator<AddSellerUserCommand>, AddSellerUserValidator>();
            services.AddScoped<IValidator<UserLogInCommand>, UserLogInValidator>();
            services.AddScoped<IValidator<ResetPasswordCommand>, ResetPasswordCommandValidator>();

            return services;
        }
    }
}
