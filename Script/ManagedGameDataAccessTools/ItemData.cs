using GameAccessTools.SourceGenerator.Attributes;
using UnrealSharp.GameDataAccessTools;

namespace ManagedGameDataAccessTools;

[ReferenceFor(typeof(FItem), IsReadOnly = true)]
public partial struct ItemData;