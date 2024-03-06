using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InStockWebAppDAL.Entities.Configurations;

public class UserPaymentConfig : IEntityTypeConfiguration<UserPayment>
{
    public void Configure(EntityTypeBuilder<UserPayment> builder)
    {
        builder.ToTable("UserPayment")
            .HasKey("Id");
        builder.Property(p => p.Provider)
            .HasMaxLength(100);
        builder.Property(p => p.AccountNumber)
            .HasMaxLength(12);
    }
}