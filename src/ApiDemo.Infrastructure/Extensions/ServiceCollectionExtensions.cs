// -------------------------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Extensions;

using Application.Repositories;
using Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PipelineBehaviors;
using Repositories;
using Services;

public static class ServiceCollectionExtensions
{
    public static T GetRequiredService<T>(this IServiceCollection services) where T : class
    {
        var serviceProvider = services.BuildServiceProvider();

        var options = serviceProvider.GetRequiredService<T>();

        return options;
    }

    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services.AddRepositories();

        AddServices(services);

        services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepositoryMock>();
        services.AddScoped<IEventRepository, EventRepositoryMock>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IApplicationContextService, HttpApplicationContextService>();
        services.AddScoped<ITokenService, TokenService>();
    }
}