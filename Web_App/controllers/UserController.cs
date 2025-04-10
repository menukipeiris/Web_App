public class UsersController : Controller
{
    private readonly IUserService _userService;
    private const int PageSize = 10;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index(int page = 1, string searchTerm = "")
    {
        try
        {
            ApiResponse apiResponse;

            if (string.IsNullOrEmpty(searchTerm))
            {
                apiResponse = await _userService.GetUsersAsync(page, PageSize);
            }
            else
            {
                apiResponse = await _userService.SearchUsersAsync(searchTerm, page, PageSize);
            }

            var viewModel = new UserViewModel
            {
                Users = apiResponse.Results,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(apiResponse.Info.Results / (double)PageSize),
                SearchTerm = searchTerm
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "An error occurred while fetching user data. Please try again later.";
            return View(new UserViewModel());
        }
    }
}