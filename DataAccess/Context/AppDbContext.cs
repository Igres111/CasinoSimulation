using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<DigitalItems> DigitalItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<LootBox> LootBoxes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DigitalItems>()
                .HasOne(d => d.User)
                .WithMany(u => u.DigitalItems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionHistory>()
                .HasOne(th => th.User)
                .WithMany(u => u.TransactionHistories)
                .HasForeignKey(th => th.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransactionHistory>()
                .HasOne(th => th.Item)
                .WithMany(di => di.TransactionHistories)
                .HasForeignKey(th => th.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LootBoxDigitalItem>()
                .HasKey(lbd => new { lbd.LootBoxId, lbd.DigitalItemId });

        }

    }
}
