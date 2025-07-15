using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;

namespace ManagedGameDataAccessTools.DataRetrieval;

public interface IGameDataEntry {
  FName Id { get; }
}