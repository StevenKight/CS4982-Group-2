using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Model;

public class UserNote
{
    #region Properties

    public int Id { get; set; }

    [Column("object_link")] public string ObjectLink { get; set; }

    public string Note { get; set; }

    #endregion
}

public class UserNoteConfiguration : IEntityTypeConfiguration<UserNote>
{
    #region Methods

    public void Configure(EntityTypeBuilder<UserNote> builder)
    {
        builder.ToTable("UserNote");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ObjectLink).IsRequired();
        builder.Property(x => x.Note).HasMaxLength(100);
    }

    #endregion
}