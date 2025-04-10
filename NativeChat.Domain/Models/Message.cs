using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NativeChat;

[Table("messages")]
public partial class Message
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("bot_id")]
    public Guid BotId { get; set; }

    [Column("content", TypeName = "text")]
    public string Content { get; set; } = null!;

    [Column("created_at")]
    public DateOnly CreatedAt { get; set; }

    [ForeignKey("BotId")]
    [InverseProperty("Messages")]
    public virtual Bot Bot { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Messages")]
    public virtual User User { get; set; } = null!;
}
