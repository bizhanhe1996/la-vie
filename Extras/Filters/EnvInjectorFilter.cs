using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LaVie.Extras.Filters;

public class GlobalsInjectorFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // only to avoid compiler error
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = context.Controller as Controller;
        if (controller != null)
        {
            controller.ViewBag.ENV_PROJECT_NAME = Env.GetString("ENV_PROJECT_NAME");
        }
    }
}
