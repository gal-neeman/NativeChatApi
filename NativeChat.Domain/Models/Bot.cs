using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NativeChat;

[Table("bots")]
public partial class Bot
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("language_id")]
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

    [InverseProperty("Bot")]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    [ForeignKey("UserId")]
    [InverseProperty("Bots")]
    public virtual User User { get; set; } = null!;
}
