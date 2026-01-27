using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Reservei.Api.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Um erro não tratado ocorreu no sistema.");

        var statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            InvalidOperationException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = GetSafeDetail(exception, statusCode),
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static string GetTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "Requisição Inválida",
            StatusCodes.Status401Unauthorized => "Não Autorizado",
            StatusCodes.Status403Forbidden => "Acesso Negado",
            StatusCodes.Status404NotFound => "Recurso Não Encontrado",
            StatusCodes.Status409Conflict => "Conflito de Regra de Negócio",
            StatusCodes.Status422UnprocessableEntity => "Erro de Processamento",
            _ => "Erro Interno do Servidor"
        };
    }

    private static string GetSafeDetail(Exception exception, int statusCode)
    {
        return statusCode == StatusCodes.Status500InternalServerError
            ? "Ocorreu um erro interno inesperado. Tente novamente mais tarde."
            : exception.Message;
    }
}
