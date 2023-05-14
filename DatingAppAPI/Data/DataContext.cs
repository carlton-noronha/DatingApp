using DatingAppAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Data
{
    public class DataContext : DbContext
    {

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserLike>()
            .HasKey(columns => new {
                columns.SourceUserId, 
                columns.TargetUserId
            });
            modelBuilder.Entity<UserLike>()
            .HasOne(source => source.SourceUser)
            .WithMany(destination => destination.LikedUsers)
            .HasForeignKey(source => source.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLike>()
            .HasOne(source => source.TargetUser)
            .WithMany(destination => destination.LikedByUsers)
            .HasForeignKey(source => source.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}