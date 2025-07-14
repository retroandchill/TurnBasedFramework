using ManagedGameDataAccessTools.DataRetrieval;
using UnrealSharp.Attributes;
using UnrealSharp.Engine;

namespace ManagedGameDataAccessTools;

[UClass]
public class ADemoActor : AActor {
  [UProperty(PropertyFlags.EditAnywhere)]
  private UDataTable DataTable { get; set; }

  protected override void BeginPlay() {
    base.BeginPlay();

    using var proxy = new DataTableProxy<ItemData>(DataTable.NativeObject);
    if (proxy.TryGetValue("POTION", out var potion)) {
      PrintString(potion.RealName);
    }
    
    foreach (var (key, item) in proxy)
    {
      PrintString($"{key} - {item.RealName} - {item.Tags.Count}");
    }
  }
}