using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NativeChat;

[Table("messages")]
public partial class Message
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("receiver_id")]
    public Guid ReceiverId { get; set; }

    [Column("sender_id")]
    public Guid SenderId { get; set; }
    
    [Column("content", TypeName = "text")]
    public string Content { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
