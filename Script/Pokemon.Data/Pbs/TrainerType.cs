using UnrealSharp.Attributes;

namespace Pokemon.Data.Pbs;

[UEnum]
public enum ETrainerGender : byte
{
    /// <summary>
    /// Represents a male trainer,
    /// </summary>
    Male,

    /// <summary>
    /// Represents a female trainer
    /// </summary>
    Female,

    /// <summary>
    /// Represents a trainer of unknown gender
    /// </summary>
    Unknown,

    /// <summary>
    /// Represents a double battle trainer with a male and female member
    /// </summary>
    Mixed
}

public class TrainerType
{
    
}