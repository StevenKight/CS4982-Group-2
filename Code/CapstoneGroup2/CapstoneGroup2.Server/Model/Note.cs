using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CapstoneGroup2.Server.Model;

public class Note
{
    #region Properties

    public int NoteId { get; set; }

    public int SourceId { get; set; }

    public string Username { get; set; }

    public string NoteText { get; set; }

    public string TagsString { get; set; }

    public List<string> Tags => this.TagsString.Split(",").ToList();

    public DateTime NoteDate { get; set; }

    #endregion
}

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    #region Methods

    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("Note");
        builder.HasKey(n => n.NoteId);
        builder.Property(n => n.NoteId).HasColumnName("note_id");
        builder.Property(n => n.SourceId).HasColumnName("source_id");
        builder.Property(n => n.Username).HasColumnName("username");
        builder.Property(n => n.NoteText).HasColumnName("note_text");
        builder.Property(n => n.TagsString).HasColumnName("tags");
        builder.Property(n => n.NoteDate).HasColumnName("note_date");
    }

    #endregion
}