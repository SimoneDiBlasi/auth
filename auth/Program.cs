using auth.Core.Interfaces;
using auth.Core.Models.AuthorizationRequirements;
using auth.Handlers.AuthorizationRequirements;
using auth.Handlers.Login;
using auth.Handlers.Logout;
using auth.Handlers.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//
var mvcBuilder = builder.Services.AddControllersWithViews();
var controllerAssembly = Assembly.Load("auth.API");
mvcBuilder.AddApplicationPart(controllerAssembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AuthDbContext>(); ; //in futuro vedere se inserendo le option con la connection string le migrazioni non rompono 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Configura l'autenticazione JWT per Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
            new string[] {}
        }
            });
});
var secretKey = builder.Configuration.GetValue<string>("SecretKey");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Indica se il token deve essere convalidato rispetto alla chiave di firma dell'emittente.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey ?? string.Empty)), // Specifica la chiave di firma utilizzata per convalidare il token
        ValidateLifetime = true, // Indica se la durata del token deve essere convalidata
        ValidateIssuer = false, //Indica se l'emittente del token deve essere convalidato
        ValidateAudience = false, // Indica se l'audience del token deve essere convalidata.
        ClockSkew = TimeSpan.Zero,
    };
});

builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.Cookie.Name = "MyCookieAuth";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
    options.AddPolicy("SeniorDeveloperPolicy", policy =>
        policy.Requirements.Add(new SeniorRequirement { ExperienceYear = 5 }));
});

builder.Services.AddScoped<ILogin, AuthenticationHandlers>();
builder.Services.AddScoped<ILogout, LogoutHandlers>();
builder.Services.AddScoped<ISignup, SignupHandlers>();
builder.Services.AddScoped<IAuthorizationHandler, AuthorizationRequirementsHandler>();
// Questo viene fatto inoltre per creare delle classi custom come la tua identity user o identity role che utilizzano la interfaccia di identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;


}).AddEntityFrameworkStores<AuthDbContext>();
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
