using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Wonderlust.Infrastructure.Extensions;
using Wonderlust.Infrastructure.Repositories;
using Scrutor;
using Wonderlust.API.Mappings;
using Wonderlust.Application.Features.Users.Commands.CreateUser;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly)
);

builder.Services.AddAutoMapper(
    [typeof(CreateUserCommandHandler).Assembly, typeof(UserMappingProfile).Assembly]
);

builder.Services.AddOpenApi();
builder.Services.AddMongoDb(builder.Configuration);

builder.Services.Scan(scan =>
    scan.FromAssemblyOf<MongoUserRepository>()
        .AddClasses(classes => classes
            .Where(t => t.Name.StartsWith("Mongo") &&
                        t.Name.EndsWith("Repository")
            )
        )
        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        .AsImplementedInterfaces()
        .WithScopedLifetime()
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();