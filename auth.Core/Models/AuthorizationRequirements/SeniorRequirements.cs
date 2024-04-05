using Microsoft.AspNetCore.Authorization;

namespace auth.Core.Models.AuthorizationRequirements
{
    public class SeniorRequirement : IAuthorizationRequirement
    {
        public int ExperienceYear { get; set; }
    }
}
