using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InStockWebAppDAL.Entities.Configurations;

public class ItemConfig : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Item")
            .HasKey("Id");
        builder.Property(p => p.TotalPrice)
            .HasPrecision(10, 2);
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
    }
}