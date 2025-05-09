using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Wonderlust.Infrastructure.Data.Mongo;

namespace Wonderlust.Infrastructure.Extensions;

public static class MongoServiceExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        MongoConfigurator.Configure();
        services.Configure<MongoDbSettings>(options =>
        {
            options.ConnectionString = configuration.GetSection("MongoDbSettings")
                .GetSection("ConnectionString").Value!;
            options.DatabaseName = configuration.GetSection("MongoDbSettings")
                .GetSection("DatabaseName").Value!;
        });

        services.AddSingleton<IMongoClient>(
            serviceProvider =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            }
        );

        services.AddScoped<IMongoDatabase>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings.DatabaseName);
        });

        return services;
    }
}