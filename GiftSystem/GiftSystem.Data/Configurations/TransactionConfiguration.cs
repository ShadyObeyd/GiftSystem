using GiftSystem.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Runtime.InteropServices.ComTypes;

namespace GiftSystem.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => new { t.SenderId, t.ReceiverId });

            builder.HasOne(t => t.Sender)
                   .WithMany(s => s.SentTransactions)
                   .HasForeignKey(t => t.SenderId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.Receiver)
                   .WithMany(r => r.ReceivedTransactions)
                   .HasForeignKey(t => t.ReceiverId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}