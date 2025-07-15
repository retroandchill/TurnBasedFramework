using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.UnrealSharpCore;

namespace ManagedGameDataAccessTools.DataRetrieval;

public interface IDataRetrievalConfig {
  IEnumerable<IDataTableProxy> CreateProxies();
}

[UClass]
public class UDataRetrievalSubsystem : UCSGameInstanceSubsystem {
  private readonly Dictionary<FName, IDataTableProxy> _proxies = new();
  
  protected override void Initialize() {
    
  }

  protected override void Deinitialize() {
    foreach (var proxy in _proxies.Values) {
      proxy.Dispose();
    }
    _proxies.Clear();
  }
}