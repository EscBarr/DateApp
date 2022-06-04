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
    public class AllCouplesModel : PageModel
    {
        public List<Tuple<User, User, DateTime>> Users { get; set; }

        private readonly ApplicationContext _db;

        public AllCouplesModel(ApplicationContext db)
        {
            _db = db;
        }

        public async Task OnGet()
        {
            var Couples = await _db.User_Couples.ToListAsync();
            Users = new List<Tuple<User, User, DateTime>>();
            foreach (var Couple in Couples)
            {
                var UserOne = await _db.Users.FirstOrDefaultAsync(u => u.Id == Couple.FirstUserId);
                var UserTwo = await _db.Users.FirstOrDefaultAsync(u => u.Id == Couple.SecondUserId);
                var date = Couple.Created;
                Users.Add(new Tuple<User, User, DateTime>(UserOne, UserTwo, date));
            }
        }

        public async Task OnPostDelete(int Firstid, int Secondid)
        {
            var Couple = await _db.User_Couples.FirstOrDefaultAsync(x => x.FirstUserId == Firstid && x.SecondUserId == Secondid);
           

            if (Couple != null)
            {
                _ = _db.User_Couples.Remove(Couple);
                await _db.SaveChangesAsync();
            }
        }
    }
}