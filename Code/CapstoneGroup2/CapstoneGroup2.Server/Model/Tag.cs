using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.Model
{
    /// <summary>
    /// Class for representing a tag
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the tag identifier.
        /// </summary>
        /// <value>
        /// The tag identifier.
        /// </value>
        public int TagID { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <value>
        /// The name of the tag.
        /// </value>
        public string TagName { get; set; }
    }
    /// <summary>
    ///  Class for configuring a tag
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;CapstoneGroup2.Server.Model.Tag&gt;" />
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        #region Methods

        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tag");
            builder.HasKey(n => n.TagID);
            builder.Property(n => n.TagID).HasColumnName("tag_id");
            builder.Property(n => n.TagName).HasColumnName("tag_name");
        }

        #endregion
    }
}
