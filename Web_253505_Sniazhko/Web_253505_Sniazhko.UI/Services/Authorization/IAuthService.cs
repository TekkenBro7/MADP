namespace Web_253505_Sniazhko.UI.Services.Authorization
{
    public interface IAuthService
    {
        Task<(bool Result, string ErrorMessage)> RegisterUserAsync(string email, string password, IFormFile? avatar);
    }
}
