using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using DotNetEnv;

namespace LaVie.Filters;

public class EnvInjectorFilter : IActionFilter
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
