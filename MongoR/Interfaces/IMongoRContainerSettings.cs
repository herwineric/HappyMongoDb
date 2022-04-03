using Microsoft.Extensions.DependencyInjection;

namespace MongoR.Interfaces;

public interface IMongoRContainerSettings
{
    public IServiceCollection ServiceCollection { get; init; }

    public ServiceLifetime Lifetime { get; init; }

    public string DbConnectionString { get; init; }
}