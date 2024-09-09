using Moq;
using tech_test.Data;
using tech_test.Models;
using tech_test.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace tech_test_unit_tests.Services;

public class AvatarServiceTests
{
    private readonly AvatarService _avatarService;
    private readonly Mock<DataDbContext> _mockDbContext;
    private readonly Mock<IMyJsonServerApi> _myJsonServerApi;

    public AvatarServiceTests()
    {
        _mockDbContext = new Mock<DataDbContext>();
        _myJsonServerApi = new Mock<IMyJsonServerApi>();
        _avatarService = new AvatarService(_mockDbContext.Object, _myJsonServerApi.Object);

        _myJsonServerApi.Setup(x => x.GetImage(It.IsAny<int>())).ReturnsAsync(new Image { Url = "my-json-server" });

        // Not tesing database results. Return null from FindAsync
        var mockDbSet = new Mock<DbSet<Image>>();
        mockDbSet.Setup(db => db.FindAsync(It.IsAny<int>())).ReturnsAsync(new Image { Url = "db1" });
        _mockDbContext.Setup(db => db.Images).Returns(mockDbSet.Object);
    }

    [Fact]
    public async Task ParseLastDigitOfUserIdentifier_ShouldSetLastDigit()
    {
        // Arrange
        string userIdentifier = "xx006";

        // Act
        await _avatarService.GetImageUrl(userIdentifier);

        // Assert
        Assert.Equal(6, _avatarService.LastDigitOfUserIdentifier);
    }

    [Fact]
    public async Task GetImageUrl_ShouldReturnDefaultImageUrl_WhenUserIdentifierIsNull()
    {
        // Act
        string result = await _avatarService.GetImageUrl(null);

        // Assert
        Assert.Equal(_avatarService.DefaultImageUrl, result);
    }

    [Fact]
    public async Task GetImageUrl_ShouldReturnImageUrl_WhenRuleA()
    {
        // Arrange
        string userIdentifier = "xx0006";

        // Act
        string result = await _avatarService.GetImageUrl(userIdentifier);

        // Assert
        Assert.Equal("my-json-server", result);
    }

    [Fact]
    public async Task GetImageUrl_ShouldReturnImageUrl_WhenDefault()
    {
        // Arrange
        string userIdentifier = "xx0000";

        // Act
        string result = await _avatarService.GetImageUrl(userIdentifier);

        // Assert
        Assert.Contains("seed=default", result);
    }

    [Fact]
    public async Task GetImageUrl_ShouldReturnImageUrl_WhenRuleB()
    {
        // Arrange
        string userIdentifier = "xx0001";

        // Act
        string result = await _avatarService.GetImageUrl(userIdentifier);

        // Assert
        Assert.Equal("db1", result);
    }

    [Fact]
    public async Task GetImageUrl_ShouldReturnImageUrl_WhenRuleC()
    {
        // Arrange
        string userIdentifier = "xx000a";

        // Act
        string result = await _avatarService.GetImageUrl(userIdentifier);

        // Assert
        Assert.Contains("vowel", result);
    }

    [Fact]
    public async Task GetImageUrl_ShouldReturnImageUrl_WhenRuleD()
    {
        // Arrange
        string userIdentifier = "xx000$";

        // Act
        string result = await _avatarService.GetImageUrl(userIdentifier);

        var regex = new Regex("seed=[1-5]");

        // Assert
        Assert.Matches(regex, result);
    }
}