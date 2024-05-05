using System.ComponentModel.DataAnnotations.Schema;

namespace DatingAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string UserName { get; set; } = "";
    }
}
