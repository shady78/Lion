using Lion.API.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Lion.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Permissions.CreateProduct,
                policy => policy.Requirements.Add(new PermissionRequirement(Permissions.CreateProduct)));
            options.AddPolicy(Permissions.EditProduct,
                policy => policy.Requirements.Add(new PermissionRequirement(Permissions.EditProduct)));
            options.AddPolicy(Permissions.DeleteProduct,
                policy => policy.Requirements.Add(new PermissionRequirement(Permissions.DeleteProduct)));
            options.AddPolicy(Permissions.ViewProduct,
                policy => policy.Requirements.Add(new PermissionRequirement(Permissions.ViewProduct)));
        });

        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        return services;
    }
}