using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace GameDataAccessTools.Core.Serialization;

public static class SerializationExtensions
{
    public static IServiceCollection ConfigureJsonSerialization(this IServiceCollection services,
                                                                Action<JsonSerializerOptions> configure)
    {
        services.TryAddSingleton(typeof(IOptions<>), typeof(OptionsManager<>));
        services.TryAddSingleton(_ => new JsonSerializerOptions());
        services.Configure(configure);
        return services;
    }
}