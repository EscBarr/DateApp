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
    public class PersonalDataModel : PageModel
    {
        [Required(ErrorMessage = "Необходимо ввести имя")]
        [Display(Name = "Имя")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Необходимо ввести фамилию")]
        [Display(Name = "Фамилия")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Необходимо ввести отчество")]
        [Display(Name = "Отчество")]
        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать пол")]
        [Display(Name = "Пол")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Необходимо указать дату рождения")]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        //[Required(ErrorMessage = "Необходимо выбрать/указать вашу религию")]
        [Display(Name = "Мирровозрение")]
        public string? ReligionType { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать/указать ваше образование")]
        [Display(Name = "Образование")]
        public string? Education { get; set; }

        [Required(ErrorMessage = "Необходимо указать ваш рост")]
        [Display(Name = "Рост")]
        public int? Height { get; set; }

        [Required(ErrorMessage = "Необходимо указать вес")]
        [Display(Name = "Вес")]
        public int? Weight { get; set; }

        [Required(ErrorMessage = "Необходимо указать ваш цвет глаз")]
        [Display(Name = "Цвет глаз")]
        public string? EyeColor { get; set; }

        [Required(ErrorMessage = "Необходимо указать ваше место проживания")]
        [Display(Name = "Город проживания")]
        public string? Adress { get; set; }

        [MaxLength(255, ErrorMessage = "Не более {1} символов")]
        [Display(Name = "Расскажите о себе")]
        public string? AboutMe { get; set; }

        [Display(Name = "Отношение к курению")]
        public string? OpinionOnSmoking { get; set; }

        [Display(Name = "Отношение к алкоголю")]
        public string? OpinionOnAlcohol { get; set; }

        public byte[]? Avatar { get; set; }

        [Display(Name = "Аватар")]
        public IFormFile? image { get; set; }

        public List<SelectListItem>? _educations;

        [Required(ErrorMessage = "Необходимо указать контактный номер телефона")]
        [Display(Name = "Контактный номер телефона")]
        public string? ContactPhone { get; set; }

        private readonly ApplicationContext _db;

        [Required(ErrorMessage = "Необходимо указать учавствуете ли вы в поиске")]
        [Display(Name = "Участие анкеты в поиске")]
        public bool IsParticipating { get; set; }

        public PersonalDataModel(ApplicationContext db)
        {
            _db = db;
        }

        //public async Task<IActionResult> Picture()
        //{
        //    return PartialViewResult("Picture", Avatar);
        //}

        public async Task OnGet()
        {
            var UserId = Int32.Parse(HttpContext.Session.GetString("userId"));

            _educations = await _db.Educations.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.Id.ToString(),
                                      Text = a.Degree
                                  }).ToListAsync();

            var appUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            Name = appUser.Name;
            Surname = appUser.Surname;
            Patronymic = appUser.Patronymic;

            if (appUser.Birthday != null)
            {
                Gender = ((bool)appUser.Gender ? 1 : 0).ToString();
                Birthday = appUser.Birthday;
            }
            else
            {
                Gender = (-1).ToString();
                Birthday = DateTime.Today;
            }
            if (appUser.Is_Participating == null)
            {
                IsParticipating = true;
            }
            else
            {
                IsParticipating = appUser.Is_Participating.Value;
            }

            ReligionType = appUser.ReligionType;
            //Education = _educations.Find(u => u.Value == appUser.EducationId.ToString()).Text;
            if (appUser.EducationId != null)
            {
                //Education = _educations.FirstOrDefault(u => u.Value == appUser.EducationId.ToString()).Value;
                Education = await _db.Educations.Where(ss => ss.Id == appUser.EducationId).Select(s => s.Id.ToString()).FirstAsync();
            }
            //Education = await _db.Educations.Where(u => u.Id == appUser.EducationId).Select(Education => Education.Degree).FirstOrDefaultAsync();
            Height = appUser.Height;
            Weight = appUser.Weight;
            EyeColor = appUser.EyeColor;
            Adress = appUser.Adress;
            AboutMe = appUser.AboutMe;
            ContactPhone = appUser.ContactPhone;
            if (appUser.OpinionOnAlcohol == null)
            {
                OpinionOnAlcohol = "Не указано";
            }
            else
            {
                OpinionOnAlcohol = appUser.OpinionOnAlcohol;
            }
            if (appUser.OpinionOnSmoking == null)
            {
                OpinionOnSmoking = "Не указано";
            }
            else
            {
                OpinionOnSmoking = appUser.OpinionOnSmoking;
            }
            Avatar = appUser.Avatar;
        }

        //public PartialViewResult OnPostImage()
        //{
        //    if (image != null)
        //    {
        //        byte[] imageData = null;
        //        // считываем переданный файл в массив байтов
        //        using (var binaryReader = new BinaryReader(image.OpenReadStream()))
        //        {
        //            imageData = binaryReader.ReadBytes((int)image.Length);
        //        }
        //        // установка массива байтов
        //        Avatar = imageData;
        //    }
        //    return Partial("_UserImagePartitial", Avatar);
        //}

        public async Task<IActionResult> OnPostEdit()
        {
            //var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var UserId = Int32.Parse(HttpContext.Session.GetString("userId"));
                var appUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
                if (appUser != null)
                {
                    appUser.Name = Name;
                    appUser.Surname = Surname;
                    appUser.Patronymic = Patronymic;
                    appUser.Gender = Gender != "0";
                    appUser.Birthday = Birthday;
                    appUser.ReligionType = ReligionType;
                    appUser.EducationId = Int32.Parse(Education);
                    appUser.Height = Height;
                    appUser.Weight = Weight;
                    appUser.EyeColor = EyeColor;
                    appUser.Adress = Adress;
                    appUser.AboutMe = AboutMe;
                    appUser.OpinionOnAlcohol = OpinionOnAlcohol;
                    appUser.OpinionOnSmoking = OpinionOnSmoking;
                    appUser.ContactPhone = ContactPhone;
                    appUser.Is_Participating = IsParticipating;
                    if (image != null)
                    {
                        byte[] imageData = null;
                        // считываем переданный файл в массив байтов
                        using (var binaryReader = new BinaryReader(image.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)image.Length);
                        }
                        // установка массива байтов
                        appUser.Avatar = imageData;
                    }
                    _db.Users.Update(appUser);
                    await _db.SaveChangesAsync();
                    return RedirectToPage("PersonalData");
                }
                else
                {
                    Response.StatusCode = 400;
                    await Response.WriteAsync("Неверно заполнены поля");
                    return RedirectToPage("PersonalData");
                }
            }
            return RedirectToPage("Index");
        }
    }
}