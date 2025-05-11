using System.Text.Json;
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapControllers();

app.Run();