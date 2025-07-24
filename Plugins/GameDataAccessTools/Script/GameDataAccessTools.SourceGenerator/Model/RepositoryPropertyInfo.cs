namespace GameAccessTools.SourceGenerator.Model;

public record RepositoryPropertyInfo
{
    public required string Type { get; init; }
    public required string Name { get; init; }
    public required string RepositoryClassName { get; init; }
    public required string? Category { get; init; }
    public bool HasCategory => Category is not null;
}