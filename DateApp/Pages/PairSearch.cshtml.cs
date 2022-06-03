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
    [BindProperties]
    public class PairSearchModel : PageModel
    {
        [Display(Name = "От")]
        public int? AgeFrom { get; set; }

        [Display(Name = "До")]
        public int? AgeTo { get; set; }

        [Display(Name = "Мирровозрение")]
        public string? ReligionType { get; set; }

        [Display(Name = "Образование")]
        public string? Education { get; set; }

        [Display(Name = "Город проживания")]
        public string? Adress { get; set; }

        [Display(Name = "Рост")]
        public int? Height { get; set; }

        [Display(Name = "Вес")]
        public int? Weight { get; set; }

        [Display(Name = "Цвет глаз")]
        public string? EyeColor { get; set; }

        [Display(Name = "Отношение к курению")]
        public string? OpinionOnSmoking { get; set; }

        [Display(Name = "Отношение к алкоголю")]
        public string? OpinionOnAlcohol { get; set; }

        private readonly ApplicationContext _db;

        public List<SelectListItem>? _educations;

        public List<User>? Users { get; set; }//Загружаем всех пользователей

        //private string Current_Sql { get; set; }

        public PairSearchModel(ApplicationContext db)
        {
            _db = db;
        }

        public async Task OnGet()
        {
            var UserId = Int32.Parse(HttpContext.Session.GetString("userId"));
            var appUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            _educations = await _db.Educations.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.Id.ToString(),
                                     Text = a.Degree
                                 }).ToListAsync();
            _educations.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "Не выбрано"
            });

            //var Current_Sql = SqlCommandGen.GenBaseRequest(!appUser.Gender.Value);
            //Check_For_Null_Fields(ref Current_Sql);
            //Users = _db.Users.FromSqlRaw(Current_Sql).ToList();
            //return new PartialViewResult
            //{
            //    ViewName = "_UserList",
            //    ViewData = this.ViewData
            //};
        }

        private void Check_For_Null_Fields(ref string Query)
        {
            if (AgeFrom is not null && AgeTo is not null)
            {
                SqlCommandGen.Add_DateCheck(ref Query, AgeFrom.ToString(), AgeTo.ToString());
            }
            if (ReligionType != "Не выбрано")
            {
                SqlCommandGen.Add_ReligionCheck(ref Query, ReligionType);
            }
            if (Education != "0" && Education != " ")
            {
                SqlCommandGen.Add_EducationCheck(ref Query, Education);
            }
            if (Adress is not null)
            {
                SqlCommandGen.Add_AdressCheck(ref Query, Adress);
            }
            if (Height is not null)
            {
                SqlCommandGen.Add_HeightCheck(ref Query, Height.ToString());
            }
            if (Weight is not null)
            {
                SqlCommandGen.Add_WeightCheck(ref Query, Weight.ToString());
            }
            if (EyeColor != "Не выбрано")
            {
                SqlCommandGen.Add_EyeColorCheck(ref Query, EyeColor);
            }
            if (OpinionOnSmoking != "Не выбрано")
            {
                SqlCommandGen.Add_OpinionOnSmokingCheck(ref Query, OpinionOnSmoking);
            }
            if (OpinionOnAlcohol != "Не выбрано")
            {
                SqlCommandGen.Add_OpinionOnAlcoholCheck(ref Query, OpinionOnAlcohol);
            }
        }

        public async Task<IActionResult> OnPostSearch()
        {
            var UserId = Int32.Parse(HttpContext.Session.GetString("userId"));
            var appUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            var Current_Sql = SqlCommandGen.GenBaseRequest(!appUser.Gender.Value);
            Check_For_Null_Fields(ref Current_Sql);

            Users = _db.Users.FromSqlRaw(Current_Sql).ToList();

            //return new PartialViewResult
            //{
            //    ViewName = "_UserList",
            //    ViewData = this.ViewData
            //};
            return Partial("_UserList", Users);
        }

        public async Task OnPostLike(int id)
        {
            var UserId = Int32.Parse(HttpContext.Session.GetString("userId"));
            var appUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            var LikedUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (!appUser.SentLikes.Contains(id.ToString()))
            {
                appUser.SentLikes.Add(id.ToString());
                _db.Users.Update(appUser);
                await _db.SaveChangesAsync();
            }

            if (!LikedUser.RecievedLikes.Contains(UserId.ToString()))
            {
                LikedUser.RecievedLikes.Add(UserId.ToString());
                _db.Users.Update(LikedUser);
                await _db.SaveChangesAsync();
            }

            if (LikedUser.SentLikes.Contains(UserId.ToString()))//В жопу эти триггеры
            {
                var Pair = new Couples { FirstUserId = UserId, SecondUserId = id, Created = DateTime.UtcNow };
                await _db.User_Couples.AddAsync(Pair);
                await _db.SaveChangesAsync();
            }
        }

        public async Task OnPostDisLike(int id)
        {
            var UserId = Int32.Parse(HttpContext.Session.GetString("userId"));
            var appUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            var DisLikedUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            appUser.RecievedLikes.Remove(id.ToString());
            DisLikedUser.SentLikes.Remove(UserId.ToString());
            _db.Users.Update(appUser);
            _db.Users.Update(DisLikedUser);
            await _db.SaveChangesAsync();
        }
    }
}