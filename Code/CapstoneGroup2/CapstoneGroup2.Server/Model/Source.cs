using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Model;

public enum SourceType
{
    #region Enum members

    Pdf = 1,
    Vid = 2

    #endregion
}

public class Source
{
    #region Properties

    public int SourceId { get; set; }

    public string Username { get; set; }

    public string Type { get; set; }

    public SourceType NoteType => (SourceType)Enum.Parse(typeof(SourceType), this.Type, true);

    public string Name { get; set; }

    public bool IsLink { get; set; }

    public string Link { get; set; }

    #endregion
}

public class SourceConfiguration : IEntityTypeConfiguration<Source>
{
    #region Methods

    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.ToTable("Source");
        builder.HasKey(s => s.SourceId);
        builder.Property(s => s.SourceId).HasColumnName("source_id");
        builder.Property(s => s.Username).HasColumnName("username");
        builder.Property(s => s.Type).HasColumnName("type");
        builder.Property(s => s.Name).HasColumnName("name");
        builder.Property(s => s.IsLink).HasColumnName("is_link");
        builder.Property(s => s.Link).HasColumnName("link");
    }

    #endregion
}