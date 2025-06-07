using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NativeChat;

[Table("messages")]
public partial class Message
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("receiverId")]
    [Column("receiver_id")]
    public Guid ReceiverId { get; set; }

    [JsonPropertyName("senderId")]
    [Column("sender_id")]
    public Guid SenderId { get; set; }

    [JsonPropertyName("content")]
    [Column("content", TypeName = "nvarchar(max)")]
    public string Content { get; set; } = null!;

    [JsonPropertyName("createdAt")]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
