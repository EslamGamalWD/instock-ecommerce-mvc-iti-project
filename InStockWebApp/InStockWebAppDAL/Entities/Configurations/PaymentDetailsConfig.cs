using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InStockWebAppDAL.Entities.Configurations;

public class PaymentDetailsConfig : IEntityTypeConfiguration<PaymentDetails>
{
    public void Configure(EntityTypeBuilder<PaymentDetails> builder)
    {
        builder.ToTable("PaymentDetails")
            .HasKey("Id");
        builder.Property(p => p.Provider)
            .HasMaxLength(100);
    }
}