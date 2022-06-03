using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DateApp.EntityContext;
using System.ComponentModel.DataAnnotations;
using DateApp.Services;

namespace DateApp.Pages
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        [Required(ErrorMessage = "Необходимо указать почту")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Повторно введите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? PasswordConfirm { get; set; }

        private readonly IAuthService _authService;
        private readonly ApplicationContext _db;

        public RegisterModel(IAuthService authService, ApplicationContext db)
        {
            _authService = authService;
            _db = db;
        }

        /// <summary>
        /// Send response when user successfully register/login
        /// </summary>
        /// <param name="email">email from request</param>
        /// <param name="appUser">user</param>
        /// <returns></returns>
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

        public async Task<IActionResult> OnPostRegister()
        {
            var appUser = new User { Email = Email, PwHash = _authService.GetHashedPassword(Password), UserRole = "User", SentLikes = new List<string>(), RecievedLikes = new List<string>(), Avatar = System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAMgAAADIBAMAAABfdrOtAAAAG1BMVEXMzMyWlpacnJyqqqrFxcWxsbGjo6O3t7e+vr6He3KoAAAACXBIWXMAAA7EAAAOxAGVKw4bAAACmElEQVR4nO3av2/aQBQH8O8ZbDPahJCOtpO2jE6aSB0PiDIbhswgIZHRphJdSfqP994ZEpwAigClrfr9SLyLeY7f/bIXAxAREREREREREREREREREREREf0/6ldfNDBr5svwrJ9MsDGxB6cZt+CG8VkZXsRJqDcm9jBI3RN0LxrNMjwz3w7STYl9jDWaKHJcLmwAihRFBngtqKiSOEBiJkYqPWY2mPn7JN/BieAElcQBfsKMJAa6qQ1Ao+3KCvi5jGQ9cRD/RLreHdlgjsdFVCbMxFUShxi0zFigIhvM8WNYblg31NXEAWrNRbXDtXaZuOzgWCOpxxNUp94p92uvrXGsNfFOYPdxkdlgDorYbqVx+jqxv0KuVbkdkl4Ldju8TuzPdBOVG9s9deX6nlQ61h0fJ0lSeUT1zN2hZWWSJDrWsys0Kg/bQWanUJnvg2M9hYmIiIiIiIiIiIiIiIjoD1HLz79SJGxev69IYI7ktaa8mHcCOTT68RS4CydQkX3PvUVQ//b+Ija2F5gvi9Q/618m3A61eoA/3l4EPVydw9G++cNNUqhhIE0tGUiR4Tm83GTWipyNMF0WqWVlcFM1h1fsKuJO7rJa3sUP3N/eQE1sc38rHVMm1Yhws14k6DSiZRFHl6EeKWfxtH0BzXQ5uh410mH+HRemz0rb5gJdKWJSmJrP6vWzFJl7+bKIWoVA+aPp9iJm4W0Ho84okgvJ/0gTrNYkwFMtr4zEmeHtSPA12jESOUu6O3oYme7bK0mzNhJniEoR9xRv1wSDbGcRWRMM81mGue7LlaR5WRP4Z9Ui9sDGtd2FXTeVnGx2Fx51V8OPO3KqNC+7C360oUg5pf1wdZ/sLPIO3kf8mGD2ATVU5wOKEP09fgOLdXyF2B0MogAAAABJRU5ErkJggg==") };

            try
            {
                await _db.Users.AddAsync(appUser);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetString("Token", _authService.GetAuthData(Email, appUser));
                HttpContext.Session.SetString("username", $"{appUser.Name} {appUser.Surname} {appUser.Patronymic}");
                HttpContext.Session.SetString("userId", $"{appUser.Id}");
                HttpContext.Session.SetString("roles", $"{appUser.UserRole}");
                return RedirectToPage("PersonalData");
            }
            catch (Exception)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Неверные данные пользователя");
                return RedirectToPage("Register");
            }
        }
    }
}