using Microsoft.AspNetCore.Authorization;

namespace auth.Core.Models.AuthorizationRequirements
{
    public class UniqueIdentifierRequirements : IAuthorizationRequirement
    {
        public required string Id { get; set; }
    }
}
