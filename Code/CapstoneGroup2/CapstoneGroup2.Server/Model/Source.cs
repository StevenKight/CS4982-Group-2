using System.ComponentModel.DataAnnotations.Schema;
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
    public int SourceId { get; set; }

    public string Type { get; set; }

    public SourceType NoteType => (SourceType)Enum.Parse(typeof(NoteType), this.Type, true);

    public string Name { get; set; }

    public bool IsLink { get; set; }

    public string Link { get; set; }
}

public class SourceConfiguration : IEntityTypeConfiguration<Source>
{
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.ToTable("Source");
        builder.HasKey(s => s.SourceId);
        builder.Property(s => s.SourceId).HasColumnName("source_id");
        builder.Property(s => s.Type).HasColumnName("type");
        builder.Property(s => s.Name).HasColumnName("name");
        builder.Property(s => s.IsLink).HasColumnName("is_link");
        builder.Property(s => s.Link).HasColumnName("link");
    }
}   