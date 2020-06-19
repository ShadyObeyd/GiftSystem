using GiftSystem.Models.DomainModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftSystem.App.Areas.Identity.Data
{
    public class GiftSystemContext : IdentityDbContext<GiftSystemUser>
    {
        public GiftSystemContext(DbContextOptions<GiftSystemContext> options)
            : base(options)
        {

        }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GiftSystemUser>()
                   .HasIndex(u => u.PhoneNumber)
                   .IsUnique();

            builder.Entity<Transaction>()
                   .HasOne(t => t.Sender)
                   .WithMany(s => s.SentTransactions)
                   .HasForeignKey(t => t.SenderId);

            builder.Entity<Transaction>()
                   .HasOne(t => t.Receiver)
                   .WithMany(s => s.ReceivedTransactions)
                   .HasForeignKey(t => t.ReceiverId);

            base.OnModelCreating(builder);
        }
    }
}