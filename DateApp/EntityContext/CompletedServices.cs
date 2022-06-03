using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DateApp.EntityContext
{
    public class CompletedServices
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Column(TypeName = "jsonb")]
        public string Usedservices { get; set; }
    }
}