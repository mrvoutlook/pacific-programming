using tech_test.Data;

namespace tech_test.Services;

public class AvatarService : IAvatarService
{
    private readonly DataDbContext _dbContext;
    private readonly IMyJsonServerApi _myJsonServerApi;

    public AvatarService(DataDbContext dataDbContext, IMyJsonServerApi myJsonServerApi)
    {
        _dbContext = dataDbContext;
        _myJsonServerApi = myJsonServerApi;

        DefaultImageUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
    }

    public string DefaultImageUrl { get; }

    public string ImageUrl { get; private set; } = string.Empty;

    public int LastDigitOfUserIdentifier { get; private set; }

    public string UserIdentifier { get; private set; } = string.Empty;

    public async Task<string> GetImageUrl(string? userIdentifier)
    {
        if (userIdentifier != null)
        {
            UserIdentifier = userIdentifier.ToLower();

            ParseLastDigitOfUserIdentifier();

            // If none of the these conditions are met, display the default image
            await EvaluateRulesAndSetProperties();
        }

        return !string.IsNullOrEmpty(ImageUrl) ? ImageUrl : DefaultImageUrl;
    }

    private async Task EvaluateRulesAndSetProperties()
    {
        if (await UrlRuleA())
            return;
        
        if (await UrlRuleB())
            return;
        
        if (UrlRuleC())
            return;
        
        if (UrlRuleD())
            return;
    }

    private void ParseLastDigitOfUserIdentifier()
    {
        char lastDigitOfUserIdentifier = UserIdentifier[UserIdentifier.Length - 1];

        int temp = -1;

        if (char.IsDigit(lastDigitOfUserIdentifier) && 
            int.TryParse(lastDigitOfUserIdentifier.ToString(), out temp))
        {
            LastDigitOfUserIdentifier = temp;
        }
    }

    /// <summary>
    /// If the last character of the user identifier is [6, 7, 8, 9], retrieve the 
    /// corresponding image URL from remote service for the last digit of 
    /// the identifier
    /// </summary>
    private async Task<bool> UrlRuleA()
    {
        int[] validDigits = { 6, 7, 8, 9 };

        if (validDigits.Contains(LastDigitOfUserIdentifier))
        {       
            var image = await _myJsonServerApi.GetImage(LastDigitOfUserIdentifier);

            if (image != null)
            {
                ImageUrl = image.Url;

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// If the user last character of the user identifier is [1, 2, 3, 4, 5], retrieve 
    /// the image URL from the database where the images.id value matches the last digit 
    /// of the identifier
    /// </summary>
    private async Task<bool> UrlRuleB()
    {
        int[] containsDigits = [1, 2, 3, 4, 5];

        if (containsDigits.Contains(LastDigitOfUserIdentifier))
        {
            var image = await _dbContext.Images.FindAsync(LastDigitOfUserIdentifier); ;

            if(image != null) 
            {
                ImageUrl = image.Url;

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// If the user identifier contains at least one vowel character ( aeiou ), display 
    /// the corresponding image 
    /// </summary>
    private bool UrlRuleC()
    {
        bool containsVowel = UserIdentifier.Any("aeiou".Contains);

        if (containsVowel)
        {
            ImageUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed=vowel&size=150";

            return true;
        }

        return false;
    }

    /// <summary>
    /// If the user identifier contains a non-alphanumeric character, pick a random 
    /// number between 1-5 and display the image with the appropriate seed
    /// </summary>
    private bool UrlRuleD()
    {
        bool containsNonAlphanumeric = UserIdentifier.Any(ch => !char.IsLetterOrDigit(ch));

        if (containsNonAlphanumeric)
        {
            Random random = new Random();

            int randomNumber = random.Next(1, 6); // 1 is inclusive, 6 is exclusive

            ImageUrl = $"https://api.dicebear.com/8.x/pixel-art/png?seed={randomNumber}&size=150";

            return true;
        }

        return false;
    }
}
