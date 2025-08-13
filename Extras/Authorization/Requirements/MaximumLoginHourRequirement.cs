using Microsoft.AspNetCore.Authorization;

namespace LaVie.Extras.Authorization.Requirements;

public class MaximumLoginHourRequirement : IAuthorizationRequirement
{
    public int MaxHour { get; }

    public MaximumLoginHourRequirement(int maxHour)
    {
        MaxHour = maxHour;
    }
}
