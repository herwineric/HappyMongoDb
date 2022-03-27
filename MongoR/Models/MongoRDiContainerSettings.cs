using Microsoft.Extensions.DependencyInjection;

namespace MongoR.Models;

public record MongoRDiContainerSettings(IServiceCollection ServiceCollection, ServiceLifetime Lifetime);