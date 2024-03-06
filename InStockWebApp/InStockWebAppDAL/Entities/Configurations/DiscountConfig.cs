using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InStockWebAppDAL.Entities.Configurations;

public class DiscountConfig : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.ToTable("Discount")
            .HasKey("Id");
        builder.Property(p => p.Name)
            .HasMaxLength(80);
        builder.Property(p => p.Description)
            .HasMaxLength(250);
    }
}