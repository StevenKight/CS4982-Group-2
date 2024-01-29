using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Dal;

public class DataContext : DbContext
{
    #region Properties

    public DbSet<UserNote> UserNotes { get; set; } = null!;

    #endregion

    #region Constructors

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    #endregion

    #region Methods

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserNote>().ToTable("UserNote");
    }

    #endregion
}