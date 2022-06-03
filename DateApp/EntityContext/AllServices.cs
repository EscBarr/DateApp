using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DateApp.EntityContext
{
    public class AllServices
    {
        [Required]
        [Column(TypeName = "jsonb")]
        public string Services { get; set; }
    }
}