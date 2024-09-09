using tech_test.Models;

namespace tech_test.Services;

public interface IMyJsonServerApi
{
    Task<Image?> GetImage(int lastDigitOfUserIdentifier);
}
