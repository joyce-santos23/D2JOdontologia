using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

public class InvalidJsonInputFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        try
        {
            context.HttpContext.Request.Body.Position = 0; // Reseta o stream do body
        }
        catch (JsonException ex)
        {
            context.Result = new BadRequestObjectResult(new
            {
                error = "Invalid JSON format",
                details = ex.Message // Mensagem de erro detalhada
            });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Nada a fazer após a execução
    }
}
