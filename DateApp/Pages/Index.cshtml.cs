using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DateApp.EntityContext;
using System.ComponentModel.DataAnnotations;
using DateApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace DateApp.Pages
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        [Required(ErrorMessage = "Необходимо ввести почту")]
        [Display(Name = "Email")]
        public string? Email { get; set; } = "email";

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; } = "word";

        private readonly IAuthService _authService;
        private readonly ApplicationContext _db;

        public LoginModel(IAuthService authService, ApplicationContext db)
        {
            _authService = authService;
            _db = db;
        }

        //private async Task SendIdentityResponse(string email, User appUser)
        //{
        //    var response = new
        //    {
        //        username = $"{appUser.Name} {appUser.Surname} {appUser.Patronymic}",
        //        userId = appUser.Id,
        //        roles = appUser.UserRole
        //    };

        //    // сериализация ответа
        //    Response.StatusCode = 200;
        //    Response.ContentType = "application/json";
        //    HttpContext.Session.SetString("Token", _authService.GetAuthData(email, appUser));
        //    await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        //}

        public async Task<IActionResult> OnPostLogin()
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (appUser != null && _authService.ValidateUserPassword(appUser.PwHash, Password))
            {
                HttpContext.Session.SetString("Token", _authService.GetAuthData(Email, appUser));
                HttpContext.Session.SetString("username", $"{appUser.Name} {appUser.Surname} {appUser.Patronymic}");
                HttpContext.Session.SetString("userId", $"{appUser.Id}");
                HttpContext.Session.SetString("roles", $"{appUser.UserRole}");
                if (appUser.UserRole == "Admin")
                {
                    return RedirectToPage("AdminPanel");
                }
                if (appUser.Name == null)
                {
                    return RedirectToPage("PersonalData");
                }

                return RedirectToPage("Profile", new { id = appUser.Id });
            }
            else
            {
                ModelState.AddModelError("", "Неверная почта или пароль");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostLogout()
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == Email);
            HttpContext.Session.Clear();
            return RedirectToPage("Index");
        }
    }
}