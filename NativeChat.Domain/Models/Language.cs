using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NativeChat;

[Table("language")]
public partial class Language
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("language")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("language_native")]
    [StringLength(50)]
    public string NativeName { get; set; } = null!;

    [InverseProperty("Language")]
    [JsonIgnore]
    public virtual ICollection<Bot> Bots { get; set; } = new List<Bot>();
}
