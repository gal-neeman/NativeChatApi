using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NativeChat;

[Table("bots")]
public partial class Bot
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("language_id")]
    [JsonIgnore]
    public int LanguageId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [Column("recap_prompt", TypeName = "text")]
    public string? RecapPrompt { get; set; }

    [Column("created_at")]
    public DateOnly CreatedAt { get; set; }

    [ForeignKey("LanguageId")]
    [InverseProperty("Bots")]
    public virtual Language Language { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Bots")]
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
