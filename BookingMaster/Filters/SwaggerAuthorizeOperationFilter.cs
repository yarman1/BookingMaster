using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
// Remove swagger authorization on anonymous routes
namespace BookingMaster.Filters
{
    public class SwaggerAuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            bool hasAllowAnonymous = context.MethodInfo.GetCustomAttributes(true)
            .OfType<AllowAnonymousAttribute>()
            .Any()
            || 
            context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
            .OfType<AllowAnonymousAttribute>()
            .Any() == true;

            if (hasAllowAnonymous)
                return;

            operation.Security =
        [
            new() {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            }
        ];
        }
    }

}