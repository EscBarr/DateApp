using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DateApp.EntityContext;
using System.ComponentModel.DataAnnotations;
using DateApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.IO;

namespace DateApp.Pages
{
    [BindProperties]
    public class ControlAdminModel : PageModel
    {
        public List<User> Users { get; set; }

        private readonly ApplicationContext _db;
        private readonly IAuthService _authService;

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

        public ControlAdminModel(ApplicationContext db, IAuthService authService)
        {
            _db = db;
            _authService = authService;
        }
        public async Task OnGet()
        {
            var temp = await _db.Users.ToListAsync();
            Users = temp.FindAll(u => u.UserRole == "Admin");
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var temp = await _db.Users.ToListAsync();
            Users = temp.FindAll(u => u.UserRole == "Admin");
            if (Users.Count > 1)
            {
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user != null)
                {
                    _db.Users.Remove(user);
                    await _db.SaveChangesAsync();
                }
            }
            else
            {
                ModelState.AddModelError("", "Нельзя удалить последнего администратора");
                return Page();
            }
            return Page();
        }

        public async Task OnPostAdd()
        {
            if (ModelState.IsValid)
            {
                var appUser = new User { Email = Email, PwHash = _authService.GetHashedPassword(Password), UserRole = "Admin", SentLikes = new List<string>(), RecievedLikes = new List<string>(), Avatar = System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAMgAAADIBAMAAABfdrOtAAAAG1BMVEXMzMyWlpacnJyqqqrFxcWxsbGjo6O3t7e+vr6He3KoAAAACXBIWXMAAA7EAAAOxAGVKw4bAAACmElEQVR4nO3av2/aQBQH8O8ZbDPahJCOtpO2jE6aSB0PiDIbhswgIZHRphJdSfqP994ZEpwAigClrfr9SLyLeY7f/bIXAxAREREREREREREREREREREREf0/6ldfNDBr5svwrJ9MsDGxB6cZt+CG8VkZXsRJqDcm9jBI3RN0LxrNMjwz3w7STYl9jDWaKHJcLmwAihRFBngtqKiSOEBiJkYqPWY2mPn7JN/BieAElcQBfsKMJAa6qQ1Ao+3KCvi5jGQ9cRD/RLreHdlgjsdFVCbMxFUShxi0zFigIhvM8WNYblg31NXEAWrNRbXDtXaZuOzgWCOpxxNUp94p92uvrXGsNfFOYPdxkdlgDorYbqVx+jqxv0KuVbkdkl4Ldju8TuzPdBOVG9s9deX6nlQ61h0fJ0lSeUT1zN2hZWWSJDrWsys0Kg/bQWanUJnvg2M9hYmIiIiIiIiIiIiIiIjoD1HLz79SJGxev69IYI7ktaa8mHcCOTT68RS4CydQkX3PvUVQ//b+Ija2F5gvi9Q/618m3A61eoA/3l4EPVydw9G++cNNUqhhIE0tGUiR4Tm83GTWipyNMF0WqWVlcFM1h1fsKuJO7rJa3sUP3N/eQE1sc38rHVMm1Yhws14k6DSiZRFHl6EeKWfxtH0BzXQ5uh410mH+HRemz0rb5gJdKWJSmJrP6vWzFJl7+bKIWoVA+aPp9iJm4W0Ho84okgvJ/0gTrNYkwFMtr4zEmeHtSPA12jESOUu6O3oYme7bK0mzNhJniEoR9xRv1wSDbGcRWRMM81mGue7LlaR5WRP4Z9Ui9sDGtd2FXTeVnGx2Fx51V8OPO3KqNC+7C360oUg5pf1wdZ/sLPIO3kf8mGD2ATVU5wOKEP09fgOLdXyF2B0MogAAAABJRU5ErkJggg==") };
                await _db.Users.AddAsync(appUser);
                await _db.SaveChangesAsync();
            }
            
        }
    }
}
