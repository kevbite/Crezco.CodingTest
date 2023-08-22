using Crezco.CodingTest.Api.Location;
using MassTransit;
using MongoDB.Driver;

namespace Crezco.CodingTest.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<GetIpLocationConsumer>();
            x.AddInMemoryInboxOutbox();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
    
    public static IServiceCollection AddMongoDb(this IServiceCollection services)
    {
        services.AddSingleton<MongoClient>(_ => new MongoClient());
        services.AddSingleton<IMongoDatabase>(provider => provider.GetRequiredService<MongoClient>().GetDatabase("coding-test"));

        return services;
    }
}