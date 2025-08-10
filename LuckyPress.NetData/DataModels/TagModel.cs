using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyPress.NetData.DataModels;

public class TagModel
{
    [Column(TypeName = "varchar(32)")] public string Name { get; set; } = "";

    [Key]
    [Column(TypeName = "varchar(32)")]
    public string Key { get; set; } = "";

    public List<ArticleModel> Articles { get; set; } = [];
}