using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyPress.NetData.DataModels;

public class EMailModel : DataModel
{
    [Key]
    [Column(TypeName = "varchar(128)")]
    public string Id { get; set; } = "";

    [Column(TypeName = "varchar(128)")] public string EMail { get; set; } = "";
}