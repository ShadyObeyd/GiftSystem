using GiftSystem.Data.Configurations;
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
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new TransactionConfiguration());

            base.OnModelCreating(builder);
        }
    }
}