using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class CustomRequirement : IAuthorizationRequirement
{
    public string Id { get; }

    public CustomRequirement(string id)
    {
        Id = id;
    }
}

public class CustomRequirementHandler : AuthorizationHandler<CustomRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequirement requirement)
    {
        // Recupera l'ID dall'intestazione Authorization
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Confronta l'ID dall'intestazione Authorization con l'ID dal parametro personalizzato
        if (userId == requirement.Id)
        {
            context.Succeed(requirement); // L'utente è autorizzato
        }

        return Task.CompletedTask;
    }
}