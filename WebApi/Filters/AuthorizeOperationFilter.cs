using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace WebApi.Filters;

public class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Verificar si el método tiene atributo [Authorize]
        var hasAuthorize = context.MethodInfo.GetCustomAttribute<AuthorizeAttribute>() != null;

        // Verificar si el controlador tiene atributo [Authorize]
        var controllerHasAuthorize = context.MethodInfo.DeclaringType?.GetCustomAttribute<AuthorizeAttribute>() != null;

        // Verificar si el método tiene atributo [AllowAnonymous] que anula el [Authorize]
        var hasAllowAnonymous = context.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>() != null;

        // Solo aplicar seguridad si:
        // - El método tiene [Authorize] O el controlador tiene [Authorize]
        // - Y el método NO tiene [AllowAnonymous]
        if ((hasAuthorize || controllerHasAuthorize) && !hasAllowAnonymous)
        {
            operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
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
                };

            // Agregar respuestas de error de autenticación
            if (!operation.Responses.ContainsKey("401"))
            {
                operation.Responses.Add("401", new OpenApiResponse
                {
                    Description = "Unauthorized - Token requerido o inválido"
                });
            }

            if (!operation.Responses.ContainsKey("403"))
            {
                operation.Responses.Add("403", new OpenApiResponse
                {
                    Description = "Forbidden - Permisos insuficientes"
                });
            }
        }
    }
}
