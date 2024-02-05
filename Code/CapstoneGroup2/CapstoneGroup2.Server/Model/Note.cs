using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Model;

public class Note
{
    public int SourceId { get; set; }

    public string Username { get; set; }
    
    public string NoteText { get; set; }

    public string TagsString { get; set; }

    public List<string> Tags => TagsString.Split(",").ToList();
}

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("Note");
        builder.HasKey(n => new { n.SourceId, n.Username });
        builder.Property(n => n.SourceId).HasColumnName("source_id");
        builder.Property(n => n.Username).HasColumnName("username");
        builder.Property(n => n.NoteText).HasColumnName("note");
        builder.Property(n => n.TagsString).HasColumnName("tags");
    }
}
