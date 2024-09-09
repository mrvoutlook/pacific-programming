namespace tech_test.Services;

public interface IAvatarService
{
    Task<string> GetImageUrl(string? userIdentifier);
}