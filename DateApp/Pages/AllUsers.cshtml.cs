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
    public class AllUsersModel : PageModel
    {
        public List<User> Users { get; set; }

        private readonly ApplicationContext _db;

        public AllUsersModel(ApplicationContext db)
        {
            _db = db;
        }

        public async Task OnGet()
        {
            var temp = await _db.Users.ToListAsync();
            Users = temp.FindAll(u => u.UserRole == "User");
        }

        public async Task OnPostDelete(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
        }
    }
}