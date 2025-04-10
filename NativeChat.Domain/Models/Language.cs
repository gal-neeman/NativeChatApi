using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NativeChat;

[Table("language")]
public partial class Language
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("language")]
    [StringLength(50)]
    public string Language1 { get; set; } = null!;

    [Column("language_native")]
    [StringLength(50)]
    public string LanguageNative { get; set; } = null!;

    [InverseProperty("Language")]
    public virtual ICollection<Bot> Bots { get; set; } = new List<Bot>();
}
