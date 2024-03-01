using auth.Core.Interfaces;
using auth.Core.Models.AuthorizationRequirements;
using auth.Handlers.AuthorizationRequirements;
using auth.Handlers.Login;
using auth.Handlers.Logout;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.Cookie.Name = "MyCookieAuth";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SeniorDeveloperPolicy", policy =>
        policy.Requirements.Add(new SeniorRequirement { ExperienceYear = 5 }));
});


builder.Services.AddScoped<ILogin, AuthenticationHandlers>();
builder.Services.AddScoped<ILogout, LogoutHandlers>();
builder.Services.AddScoped<IAuthorizationHandler, AuthorizationRequirementsHandler>();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
