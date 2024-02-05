using CapstoneGroup2.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.Dal;

public class DocunotesDbContext : DbContext
{
    #region Properties

    public User? CurrentUser { get; set; }

    public DbSet<Note> Notes { get; set; }

    public DbSet<Shared> SharedNotes { get; set; }

    public DbSet<Source> Sources { get; set; }

    public DbSet<User> Users { get; set; }

    #endregion

    #region Constructors

    public DocunotesDbContext(DbContextOptions<DocunotesDbContext> options) : base(options)
    {
    }

    #endregion

    #region Methods

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new NoteConfiguration())
            .ApplyConfiguration(new SharedConfiguration())
            .ApplyConfiguration(new SourceConfiguration())
            .ApplyConfiguration(new UserConfiguration());
    }

    #endregion
}