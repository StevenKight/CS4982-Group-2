using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CapstoneGroup2.Server.Model;

public class Shared
{
    #region Properties

    public int SourceId { get; set; }

    public string Username { get; set; }

    public string SharedUsername { get; set; }

    public string Comment { get; set; }

    #endregion
}

public class SharedConfiguration : IEntityTypeConfiguration<Shared>
{
    #region Methods

    public void Configure(EntityTypeBuilder<Shared> builder)
    {
        builder.ToTable("Shared");
        builder.HasKey(s => new { s.SourceId, s.Username, s.SharedUsername });
        builder.Property(s => s.SourceId).HasColumnName("source_id");
        builder.Property(s => s.Username).HasColumnName("username");
        builder.Property(s => s.SharedUsername).HasColumnName("shared_username");
        builder.Property(s => s.Comment).HasColumnName("comment");
    }

    #endregion
}