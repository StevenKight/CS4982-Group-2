using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CapstoneGroup2.Server.Model;

/// <summary>
///     Source Type enums
/// </summary>
public enum SourceType
{
    #region Enum members

    /// <summary>
    ///     The PDF
    /// </summary>
    Pdf = 1,

    /// <summary>
    ///     The vid
    /// </summary>
    Vid = 2

    #endregion
}

/// <summary>
///     Source Model class
/// </summary>
public class Source
{
    #region Properties

    /// <summary>
    ///     Gets or sets the source identifier.
    /// </summary>
    /// <value>
    ///     The source identifier.
    /// </value>
    public int SourceId { get; set; }

    /// <summary>
    ///     Gets or sets the username.
    /// </summary>
    /// <value>
    ///     The username.
    /// </value>
    public string Username { get; set; }

    /// <summary>
    ///     Gets or sets the type.
    /// </summary>
    /// <value>
    ///     The type.
    /// </value>
    public string Type { get; set; }

    /// <summary>
    ///     Gets the type of the note.
    /// </summary>
    /// <value>
    ///     The type of the note.
    /// </value>
    public SourceType NoteType => (SourceType)Enum.Parse(typeof(SourceType), this.Type, true);

    /// <summary>
    ///     Gets or sets the name.
    /// </summary>
    /// <value>
    ///     The name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the description.
    /// </summary>
    /// <value>
    ///     The description.
    /// </value>
    public string? Description { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this instance is link.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this instance is link; otherwise, <c>false</c>.
    /// </value>
    public bool IsLink { get; set; }

    /// <summary>
    ///     Gets or sets the link.
    /// </summary>
    /// <value>
    ///     The link.
    /// </value>
    public string? Link { get; set; }

    /// <summary>
    ///     Gets or sets the content.
    /// </summary>
    /// <value>
    ///     The content.
    /// </value>
    public byte[]? Content { get; set; }

    /// <summary>
    ///     Gets or sets the created at.
    /// </summary>
    /// <value>
    ///     The created at.
    /// </value>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the updated at.
    /// </summary>
    /// <value>
    ///     The updated at.
    /// </value>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the authors string.
    /// </summary>
    /// <value>
    ///     The authors string.
    /// </value>
    public string? AuthorsString { get; set; }

    /// <summary>
    ///     Gets the authors.
    /// </summary>
    /// <value>
    ///     The authors.
    /// </value>
    public List<string>? Authors =>
        string.IsNullOrEmpty(this.AuthorsString) ? null : this.AuthorsString.Split("|").ToList();

    /// <summary>
    ///     Gets or sets the publisher.
    /// </summary>
    /// <value>
    ///     The publisher.
    /// </value>
    public string? Publisher { get; set; }

    /// <summary>
    ///     Gets or sets the accessed at.
    /// </summary>
    /// <value>
    ///     The accessed at.
    /// </value>
    public DateTime? AccessedAt { get; set; }

    #endregion
}

/// <summary>
/// </summary>
/// <seealso cref="Source" />
public class SourceConfiguration : IEntityTypeConfiguration<Source>
{
    #region Methods

    /// <summary>
    ///     Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.ToTable("Source");
        builder.HasKey(s => s.SourceId);
        builder.Property(s => s.SourceId).HasColumnName("source_id");
        builder.Property(s => s.Username).HasColumnName("username");
        builder.Property(s => s.Name).HasColumnName("name");
        builder.Property(s => s.Description).HasColumnName("description");
        builder.Property(s => s.Type).HasColumnName("type");
        builder.Property(s => s.IsLink).HasColumnName("is_link");
        builder.Property(s => s.Link).HasColumnName("link");
        builder.Property(s => s.CreatedAt).HasColumnName("created_at");
        builder.Property(s => s.UpdatedAt).HasColumnName("updated_at");
        builder.Property(s => s.Content).HasColumnName("content");
        builder.Property(s => s.AuthorsString).HasColumnName("authors");
        builder.Property(s => s.Publisher).HasColumnName("publisher");
        builder.Property(s => s.AccessedAt).HasColumnName("accessed_at");
    }

    #endregion
}