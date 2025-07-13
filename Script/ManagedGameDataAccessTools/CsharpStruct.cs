using GameAccessTools.SourceGenerator.Attributes;
using UnrealSharp.Attributes;

namespace ManagedGameDataAccessTools;

[UStruct]
public struct FCsharpStruct
{
    [field: UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadOnly)]
    public string Name { get; set; }
}

[ReferenceFor(typeof(FCsharpStruct))]
public partial struct CsharpStructData {
  
}