using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using DateApp.EntityContext;
using System.ComponentModel.DataAnnotations;
using DateApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DateApp.Pages
{
    [BindProperties]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, User")]
    public class ProfileModel : PageModel
    {
        public User CurrentUser { get; set; }

        private readonly ApplicationContext _db;

        public string? Education { get; set; }

        public ProfileModel(ApplicationContext db)
        {
            _db = db;
        }

        public async Task OnGet(int Id)
        {
            CurrentUser = _db.Users.Where(s => s.Id == Id).First();
            Education = await _db.Educations.Where(ss => ss.Id == CurrentUser.EducationId).Select(s => s.Degree).FirstAsync();
        }
    }
}