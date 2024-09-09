namespace tech_test.Models;

public record Image
{
    public int Id { get; init; }

    public required string Url { get; init; }
}
