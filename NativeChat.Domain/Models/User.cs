using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NativeChat;

[Table("users")]
public partial class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("first_name")]
    [StringLength(20)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(40)]
    public string LastName { get; set; } = null!;

    [Column("email")]
    [StringLength(320)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("password")]
    [StringLength(200)]
    public string Password { get; set; } = null!;

    [Column("username")]
    [StringLength(20)]
    public string Username { get; set; } = null!;

    [Column("created_at")]
    public DateOnly CreatedAt { get; set; }

    [Column("updated_at")]
    public DateOnly? UpdatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Bot> Bots { get; set; } = new List<Bot>();
}
