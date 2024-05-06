using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)"), Required]
        public string UserName { get; set; } = "";
        [Column(TypeName = "byte[]")]
        public byte[] PasswordHash { get; set; }
        [Column(TypeName = "byte[]")]
        public byte[] PasswordSalf { get; set; }

    }
}
