using System.Text.Json;
using System.Text.Json.Nodes;
using ManagedGameDataAccessToolsEditor.Interop;
using ManagedGameDataAccessToolsEditor.Serialization.Native;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;

namespace ManagedGameDataAccessToolsEditor.Serialization.Marshallers;

public static class JsonNodeMarshaller
{
    public static void ToNative(ref NativeJsonValue referenceCountedPtr, JsonNode? node)
    {
        if (node is null)
        {
            FJsonValueExporter.CallCreateJsonNull(ref referenceCountedPtr);
            return;
        }

        switch (node.GetValueKind())
        {
            case JsonValueKind.Undefined or JsonValueKind.Null:
                FJsonValueExporter.CallCreateJsonNull(ref referenceCountedPtr);
                break;
            case JsonValueKind.Object:
                var jsonObject = ToNativeJsonObject(node.AsObject());
                // This call uses a the move constructor in native code thus clearing the managed resources
                // of the array
                FJsonValueExporter.CallCreateJsonObject(ref referenceCountedPtr, ref jsonObject);
                break;
            case JsonValueKind.Array:
                var jsonArray = ToNativeJsonArray(node.AsArray());
                // This call uses a the move constructor in native code thus clearing the managed resources
                // of the array
                FJsonValueExporter.CallCreateJsonArray(ref referenceCountedPtr, ref jsonArray);
                break;
            case JsonValueKind.String:
                FJsonValueExporter.CallCreateJsonString(ref referenceCountedPtr, node.GetValue<string>());
                break;
            case JsonValueKind.Number:
                FJsonValueExporter.CallCreateJsonNumber(ref referenceCountedPtr, node.GetValue<double>());
                break;
            case JsonValueKind.True:
                FJsonValueExporter.CallCreateJsonBool(ref referenceCountedPtr, NativeBool.True);
                break;
            case JsonValueKind.False:
                FJsonValueExporter.CallCreateJsonBool(ref referenceCountedPtr, NativeBool.False);
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    private static UnmanagedArray ToNativeJsonArray(JsonArray array)
    {
        var result = new UnmanagedArray();
        foreach (var item in array)
        {
            // This native code will use the move constructor, thus emptying out the contents of the smart pointer
            var nativeValue = new NativeJsonValue();
            ToNative(ref nativeValue, item);
            JsonArrayExporter.CallAddToArray(ref result, ref nativeValue);
        }

        return result;
    }

    private static NativeJsonValue ToNativeJsonObject(JsonObject obj)
    {
        var nativeObject = new NativeJsonValue();
        FJsonObjectExporter.CallCreateJsonObject(ref nativeObject);
        foreach (var (key, value) in obj)
        {
            // This native code will use the move constructor, thus emptying out the contents of the smart pointer
            var nativeValue = new NativeJsonValue();
            ToNative(ref nativeValue, value);
            FJsonObjectExporter.CallSetField(ref nativeObject, key, ref nativeValue);
        }

        return nativeObject;
    }

    public static JsonNode? FromNative(ref NativeJsonValue nativeBuffer)
    {
        unsafe
        {
            switch (FJsonValueExporter.CallGetJsonType(ref nativeBuffer))
            {
                case EJson.None or EJson.Null:
                    return JsonValue.Create((object)null!);
                case EJson.String:
                {
                    var nativeString = new UnmanagedArray();
                    using var releaser = new StringDataReleaser(ref nativeString);
                    FJsonValueExporter.CallGetJsonString(ref nativeBuffer, ref nativeString);
                    return JsonValue.Create(StringMarshaller.FromNative((IntPtr)(&nativeString), 0));
                }
                case EJson.Number:
                    return JsonValue.Create(FJsonValueExporter.CallGetJsonNumber(ref nativeBuffer));
                case EJson.Boolean:
                    return JsonValue.Create(FJsonValueExporter.CallGetJsonBool(ref nativeBuffer).ToManagedBool());
                case EJson.Array:
                    var array = new JsonArray();
                    UnmanagedArray* unmanagedArray = null;
                    FJsonValueExporter.CallGetJsonArray(ref nativeBuffer, ref unmanagedArray);
                    for (var i = 0; i < unmanagedArray->ArrayNum; i++)
                    {
                        NativeJsonValue* nativeJsonValue = null;
                        JsonArrayExporter.CallGetAtIndex(ref *unmanagedArray, i, ref nativeJsonValue);
                        array.Add(FromNative(ref *nativeJsonValue));
                    }
                    return array;
                case EJson.Object:
                    var jsonObject = new JsonObject();
                    var jsonEnumerable = new JsonObjectEnumerable(ref nativeBuffer);
                    foreach (var keyValuePair in jsonEnumerable)
                    {
                        jsonObject.Add(keyValuePair.Key, FromNative(ref keyValuePair.Value));
                    }
                    return jsonObject;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}