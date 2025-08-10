using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyPress.NetData.DataModels;

public class UserModel : DataModel
{
    [Key]
    [Column(TypeName = "varchar(64)")]
    public string Id { get; set; } = "";

    [Column(TypeName = "varchar(64)")] public string Name { get; set; } = "";
    [Column(TypeName = "varchar(64)")] public string Email { get; set; } = "";
    [Column(TypeName = "varchar(64)")] public string Password { get; set; } = "";
    [Column(TypeName = "varchar(64)")] public string Role { get; set; } = "";
    [Column(TypeName = "varchar(64)")] public string Phone { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime LastLogin { get; set; } = DateTime.Now;
    [Column(TypeName = "varchar(64)")] public string LoginPos { get; set; } = "";

    public void Update(UserModel user)
    {
        Name = user.Name;
        Email = user.Email;
        Password = user.Password;
        Role = user.Role;
        Phone = user.Phone;
        UpdatedAt = DateTime.Now;
        LastLogin = DateTime.Now;
    }
}