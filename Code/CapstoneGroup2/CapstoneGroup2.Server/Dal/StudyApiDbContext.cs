using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Dal;

public class StudyApiDbContext : DbContext
{
    #region Properties
    
    public DbSet<Note> Notes { get; set; }

    public DbSet<Shared> SharedNotes { get; set; }

    public DbSet<Source> Sources { get; set; }

    public DbSet<User> Users { get; set; }

    #endregion

    #region Constructors

    public StudyApiDbContext(DbContextOptions<StudyApiDbContext> options) : base(options)
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