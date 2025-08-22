namespace UnrealSharp.Test.Utils;

/// <summary>
/// A generic class designed to wrap a value of a specified type, allowing for modular and reusable references.
/// </summary>
/// <typeparam name="T">
/// The type of the value being referenced.
/// </typeparam>
public sealed class ModularReference<T>(T value)
{
    /// <summary>
    /// Gets or sets the value of the modular reference.
    /// Represents the encapsulated value of type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// This property allows access to the underlying value wrapped within the <see cref="ModularReference{T}"/> instance.
    /// </remarks>
    public T Value { get; set; } = value;
}
