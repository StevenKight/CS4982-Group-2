using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Dal
{
    public class DataContext : DbContext
    {
        public DbSet<UserNote> UserNotes { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserNote>().ToTable("UserNote");
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}
