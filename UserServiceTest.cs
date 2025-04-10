// UserServiceTests.cs
public class UserServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://randomuser.me/api/")
        };
        _mockLogger = new Mock<ILogger<UserService>>();
        _userService = new UserService(_httpClient, _mockLogger.Object);
    }

    [Fact]
    public async Task GetUsersAsync_ReturnsApiResponse_WhenSuccessful()
    {
        // Arrange
        var expectedResponse = new ApiResponse
        {
            Results = new List<User>
            {
                new User
                {
                    Name = new Name { First = "John", Last = "Doe" },
                    Gender = "male",
                    Email = "john.doe@example.com"
                }
            },
            Info = new Info { Page = 1, Results = 1 }
        };

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(expectedResponse))
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _userService.GetUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Results);
        Assert.Equal("John", result.Results[0].Name.First);
    }

    [Fact]
    public async Task GetUsersAsync_ThrowsException_WhenApiFails()
    {
        // Arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _userService.GetUsersAsync());
    }
}