using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GameDataAccessTools.Core.Serialization;

public static class SerializationExtensions
{
    public static IServiceCollection ConfigureJsonSerialization(this IServiceCollection services,
                                                                Action<JsonSerializerOptions> configure)
    {
        services.TryAddSingleton<JsonSerializerOptions>();
        services.Configure(configure);
        return services;
    }
}