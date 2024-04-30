using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class AppUser {
    public int Id { get; set; }
    [Column(TypeName="nvarchar(50)")]
    public string UserName { get; set; }
}