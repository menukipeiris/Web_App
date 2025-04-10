public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserService> _logger;

    public UserService(HttpClient httpClient, ILogger<UserService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ApiResponse> GetUsersAsync(int page = 1, int results = 10)
    {
        try
        {
            var response = await _httpClient.GetAsync($"?page={page}&results={results}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(content);

            return apiResponse;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching users from API");
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error parsing API response");
            throw;
        }
    }

    public async Task<ApiResponse> SearchUsersAsync(string searchTerm, int page = 1, int results = 10)
    {
        try
        {
            var response = await _httpClient.GetAsync($"?page={page}&results={results}&seed={searchTerm}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(content);

            return apiResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users");
            throw;
        }
    }
}