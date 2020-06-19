using GiftSystem.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiftSystem.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<GiftSystemUser>
    {
        public void Configure(EntityTypeBuilder<GiftSystemUser> builder)
        {
            builder.HasIndex(u => u.PhoneNumber)
                   .IsUnique();
        }
    }
}