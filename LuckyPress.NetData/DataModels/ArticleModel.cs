using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyPress.NetData.DataModels;

public class ArticleModel : DataModel
{
    /// <summary>
    /// 默认为 Guid.NewGuid().ToString()[..5]
    /// </summary>
    [Key]
    [Column(TypeName = "varchar(128)")]
    public string Id { get; set; } = "";

    /// <summary>
    /// 路径
    /// </summary>
    [Column(TypeName = "varchar(128)")]
    public string Path { get; set; } = "";

    /// <summary>
    /// 标题
    /// </summary>
    [Column(TypeName = "varchar(32)")]
    public string Title { get; set; } = "";

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; } = "";

    [MaxLength(200)] public string Intro { get; set; } = "";

    /// <summary>
    /// 最后修改时间
    /// </summary>
    [Column(TypeName = "DATE")]
    public DateTime LastWriteTime { get; set; } = DateTime.Now;

    /// <summary>
    /// Publish | Draft | Private
    /// 发表 | 草稿 | 私密
    /// </summary>
    [Column(TypeName = "varchar(20)")]
    public string State { get; set; } = "Publish";

    public List<TagModel> Tags { get; set; } = [];

    public int Watch { get; set; }
}