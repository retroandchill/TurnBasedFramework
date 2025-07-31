using Pokemon.Data.Pbs;
using Pokemon.Editor.Model.Data.Pbs;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class MoveMapper
{
    public static UMove ToMove(this MoveInfo moveInfo, UObject? outer = null)
    {
        return moveInfo.ToMoveInitializer(outer);
    }

    public static partial MoveInfo ToMoveInfo(this UMove move);

    private static partial MoveInitializer ToMoveInitializer(this MoveInfo move, UObject? outer = null);
}
