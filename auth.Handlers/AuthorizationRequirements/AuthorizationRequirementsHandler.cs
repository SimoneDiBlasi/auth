using auth.Core.Models.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;

namespace auth.Handlers.AuthorizationRequirements
{
    public class AuthorizationRequirementsHandler : AuthorizationHandler<SeniorRequirement>
    {
        public AuthorizationRequirementsHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SeniorRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "DeveloperExperienceYear"))
                return Task.CompletedTask;

            var experienceYears = context.User.FindFirst(x => x.Type == "DeveloperExperienceYear")?.Value;


            if (experienceYears != null && int.Parse(experienceYears) >= requirement.ExperienceYear)
            {
                context.Succeed(requirement);
            }


            return Task.CompletedTask;

        }
    }
}
