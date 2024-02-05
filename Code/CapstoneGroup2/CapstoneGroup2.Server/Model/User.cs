using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Model;

public class User
{
    #region Properties

    public string Username { get; set; }

    public string? Password { get; set; }

    [NotMapped] public string? Token { get; set; }

    #endregion
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    #region Methods

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(u => u.Username);
        builder.Property(u => u.Username).HasColumnName("username");
        builder.Property(u => u.Password).HasColumnName("password");
    }

    #endregion
}