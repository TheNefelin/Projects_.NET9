using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectAuth.Application.Interfaces;

namespace WebApi.Filters;

public class ApiKeyFilter : IAsyncActionFilter
{
    private readonly string ApiKeyHeaderName = "ApiKey";
    private readonly IMaeConfigService _maeConfigService;

    public ApiKeyFilter(IMaeConfigService maeConfigService)
    {
        _maeConfigService = maeConfigService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Intentar obtener el valor de ApiKey del header
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey) || string.IsNullOrEmpty(apiKey))
        {
            // Si ApiKey no está presente o es nula, retornar un 401
            context.Result = new UnauthorizedObjectResult(new ApiResponse<object>
            {
                IsSuccess = false,
                StatusCode = 401,
                Message = "ApiKey es Requerida."
            });

            return;
        }

        // Llama al servicio para validar ApiKey en la base de datos
        var IsValid = await _maeConfigService.ValidateApiKey(apiKey);

        if (!IsValid)
        {
            context.Result = new UnauthorizedObjectResult(new ApiResponse<object>
            {
                IsSuccess = false,
                StatusCode = 401,
                Message = "ApiKey no Autorizado."
            });

            return;
        }

        // Llamar a la siguiente acción si la ApiKey está presente
        await next();
    }
}
