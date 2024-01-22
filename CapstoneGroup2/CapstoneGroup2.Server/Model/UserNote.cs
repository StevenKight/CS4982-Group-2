using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Model;

public enum NoteType
{
    #region Enum members

    Pdf = 1,
    Vid = 2

    #endregion
}

public class UserNote
{
    #region Properties

    public int Id { get; set; }

    [Column("object_link")] public string ObjectLink { get; set; }

    public string Note { get; set; }

    public string Type { get; set; }

    public NoteType NoteType => (NoteType)Enum.Parse(typeof(NoteType), this.Type, true);

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
        builder.Property(x => x.Type).HasMaxLength(3);
    }

    #endregion
}