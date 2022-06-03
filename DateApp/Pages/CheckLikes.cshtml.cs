using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DateApp.EntityContext;
using DateApp.Extensions;
using System.ComponentModel.DataAnnotations;
using DateApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.IO;

namespace DateApp.Pages
{
    public class CheckLikesModel : PageModel
    {
        private readonly ApplicationContext _db;

        public List<User>? Users { get; set; }//Загружаем всех пользователей

        public CheckLikesModel(ApplicationContext db)
        {
            _db = db;
        }

        public async Task OnGet()
        {
            var UserId = Int32.Parse(HttpContext.Session.GetString("userId"));
            var appUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            Users = _db.Users.Where(u => appUser.RecievedLikes.Contains(u.Id.ToString())).ToList();
        }
    }
}