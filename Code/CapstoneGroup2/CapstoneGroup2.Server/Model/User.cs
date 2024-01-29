using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Model;

public class User
{
    public string Username { get; set; }

    public string Password { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(u => u.Username);
        builder.Property(u => u.Username).HasColumnName("username");
        builder.Property(u => u.Password).HasColumnName("password");
    }
}