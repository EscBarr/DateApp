using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DateApp.EntityContext;
using System.ComponentModel.DataAnnotations.Schema;

namespace DateApp.EntityContext
{
    public class User
    {
        public int Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string PwHash { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public bool? Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }

        public string? UserRole { get; set; }

        //public int PassportId { get; set; }
        public string? ReligionType { get; set; }

        public int? EducationId { get; set; }

        public byte[]? Avatar { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public string? EyeColor { get; set; }
        public string? Adress { get; set; }

        [StringLength(255)]
        public string? AboutMe { get; set; }

        public string? OpinionOnSmoking { get; set; }

        public string? OpinionOnAlcohol { get; set; }

        public string? ContactPhone { get; set; }

        public List<string>? RecievedLikes { get; set; }

        public List<string>? SentLikes { get; set; }

        public bool? Is_Participating { get; set; }

        //public Passport Passport { get; set; }
        public EducationDegrees Education { get; set; }
    }
}