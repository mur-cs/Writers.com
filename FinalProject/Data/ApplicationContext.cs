using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
            //Database.EnsureDeleted();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<Comment> Comments { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Comment>()
				.HasOne(c => c.User)
				.WithMany(u => u.Comments)
				.HasForeignKey(c => c.UserId)
				.OnDelete(DeleteBehavior.Restrict); 

			modelBuilder.Entity<Comment>()
				.HasOne(c => c.Composition)
				.WithMany(comp => comp.Comments)
				.HasForeignKey(c => c.CompositionId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Composition>()
				.HasOne(c => c.User)
				.WithMany(u => u.Compositions)
				.HasForeignKey(c => c.UserId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
