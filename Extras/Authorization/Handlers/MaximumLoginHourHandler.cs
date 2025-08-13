using LaVie.Extras.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace LaVie.Extras.Authorization.Handlers;

public class MaximumLoginHourHandler : AuthorizationHandler<MaximumLoginHourRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MaximumLoginHourRequirement requirement
    )
    {
        var currentHour = DateTime.Now.Hour;
        if (currentHour <= requirement.MaxHour)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
