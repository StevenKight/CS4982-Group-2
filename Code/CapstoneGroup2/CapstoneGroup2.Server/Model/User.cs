using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CapstoneGroup2.Server.Model;

/// <summary>
/// User Model Class
/// </summary>
public class User
{
    #region Properties

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>
    /// The username.
    /// </value>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>
    /// The password.
    /// </value>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    /// <value>
    /// The token.
    /// </value>
    [NotMapped] public string? Token { get; set; }

    #endregion
}

/// <summary>
/// Class for configuring User
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;CapstoneGroup2.Server.Model.User&gt;" />
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    #region Methods

    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(u => u.Username);
        builder.Property(u => u.Username).HasColumnName("username");
        builder.Property(u => u.Password).HasColumnName("password");
    }

    #endregion
}