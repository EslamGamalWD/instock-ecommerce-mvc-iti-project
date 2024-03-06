using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InStockWebAppDAL.Entities.Configurations;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product")
            .HasKey("Id");
        builder.Property(p => p.Name)
            .HasMaxLength(200);
        builder.Property(p => p.Description)
            .HasMaxLength(250);
    }
}