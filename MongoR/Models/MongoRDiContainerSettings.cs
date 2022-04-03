using Microsoft.Extensions.DependencyInjection;
using MongoR.Interfaces;

namespace MongoR.Models;

public class MongoRDiContainerSettings : IMongoRContainerSettings
{
    public IServiceCollection ServiceCollection { get; init; }

    public ServiceLifetime Lifetime { get; init; } = ServiceLifetime.Scoped;

    public string DbConnectionString { get; init; } = null!;
};