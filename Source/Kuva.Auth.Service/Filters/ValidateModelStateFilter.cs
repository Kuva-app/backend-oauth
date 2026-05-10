using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kuva.Auth.Service.Filters;

public sealed class ValidateModelStateFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

        var problem = new ValidationProblemDetails(context.ModelState)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Payload inválido."
        };
        context.Result = new BadRequestObjectResult(problem);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
