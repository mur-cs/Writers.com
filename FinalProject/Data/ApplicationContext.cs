using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
