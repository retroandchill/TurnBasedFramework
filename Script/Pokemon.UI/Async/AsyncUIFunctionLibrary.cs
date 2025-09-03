using Pokemon.Core;
using Pokemon.Core.Executor.Display;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CommonUtilities;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;

namespace Pokemon.UI.Async;

[UClass]
public class UAsyncUIFunctionLibrary : UBlueprintFunctionLibrary
{
    [UFunction(FunctionFlags.BlueprintCallable, Category = "Messages")]
    [UMetaData("WorldContext", nameof(worldContext))]
    public static Task DisplayMessage(UObject worldContext, FText message, bool autoRemove = true, CancellationToken cancellationToken = default)
    {
        UWorldContextExtensions.WorldContext = worldContext;
        return IDisplayService.Instance.DisplayMessage(message, autoRemove, cancellationToken);
    }
}