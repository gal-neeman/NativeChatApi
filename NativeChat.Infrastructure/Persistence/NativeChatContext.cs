using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace NativeChat;

public partial class NativeChatContext : DbContext
{
    private DatabaseSettings _dbSettings;

    public NativeChatContext(IOptions<DatabaseSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }

    public NativeChatContext(DbContextOptions<NativeChatContext> options, IOptions<DatabaseSettings> dbSettings)
        : base(options)
    {
        _dbSettings = dbSettings.Value;
    }

    public virtual DbSet<Bot> Bots { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(_dbSettings.DefaultConnection);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bot>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Language).WithMany(p => p.Bots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bots_language");

            entity.HasOne(d => d.User).WithMany(p => p.Bots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bots_users");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
