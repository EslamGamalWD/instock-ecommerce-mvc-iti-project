using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InStockWebAppDAL.Entities.Configurations;

public class CartConfig : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Cart")
            .HasKey("Id");
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
    }
}