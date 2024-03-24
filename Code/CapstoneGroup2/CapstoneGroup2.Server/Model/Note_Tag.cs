using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.Model;

/// <summary>
/// Class for representing a note tag
/// </summary>
public class Note_Tag
{
    /// <summary>
    /// Gets or sets the note identifier.
    /// </summary>
    /// <value>
    /// The note identifier.
    /// </value>
    public int NoteID { get; set; }
    /// <summary>
    /// Gets or sets the tag identifier.
    /// </summary>
    /// <value>
    /// The tag identifier.
    /// </value>
    public int TagID { get; set; }
}

/// <summary>
/// Class for configuring a note tag
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;CapstoneGroup2.Server.Model.Note_Tag&gt;" />
public class NoteTagConfiguration : IEntityTypeConfiguration<Note_Tag>
{
    #region Methods

    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Note_Tag> builder)
    {
        builder.ToTable("Note_Tag");
        builder.HasKey(n => new { n.TagID, n.NoteID });
        builder.Property(n => n.TagID).HasColumnName("tag_id");
        builder.Property(n => n.NoteID).HasColumnName("note_id");
    }

    #endregion
}