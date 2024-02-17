using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CapstoneGroup2.Server.Model;

/// <summary>
/// Note model Class
/// </summary>
public class Note
{
    #region Properties

    /// <summary>
    /// Gets or sets the note identifier.
    /// </summary>
    /// <value>
    /// The note identifier.
    /// </value>
    public int NoteId { get; set; }

    /// <summary>
    /// Gets or sets the source identifier.
    /// </summary>
    /// <value>
    /// The source identifier.
    /// </value>
    public int SourceId { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>
    /// The username.
    /// </value>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the note text.
    /// </summary>
    /// <value>
    /// The note text.
    /// </value>
    public string NoteText { get; set; }

    /// <summary>
    /// Gets or sets the tags string.
    /// </summary>
    /// <value>
    /// The tags string.
    /// </value>
    public string TagsString { get; set; }

    /// <summary>
    /// Gets the tags.
    /// </summary>
    /// <value>
    /// The tags.
    /// </value>
    public List<string> Tags => this.TagsString.Split(",").ToList();

    /// <summary>
    /// Gets or sets the note date.
    /// </summary>
    /// <value>
    /// The note date.
    /// </value>
    public DateTime NoteDate { get; set; }

    #endregion
}

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;CapstoneGroup2.Server.Model.Note&gt;" />
public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    #region Methods

    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
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