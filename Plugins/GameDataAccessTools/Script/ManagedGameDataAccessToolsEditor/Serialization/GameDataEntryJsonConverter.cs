using System.Text.Json;
using System.Text.Json.Serialization;
using UnrealSharp.GameDataAccessTools;

namespace ManagedGameDataAccessToolsEditor.Serialization;

public sealed class GameDataEntryJsonConverter<TEntry> : JsonConverter<TEntry> where TEntry : UGameDataEntry
{
    public override TEntry? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, TEntry value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}