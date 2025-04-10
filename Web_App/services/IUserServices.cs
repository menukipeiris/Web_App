public interface IUserService
{
    Task<ApiResponse> GetUsersAsync(int page, int results);
    Task<ApiResponse> SearchUsersAsync(string searchTerm, int page, int results);
}