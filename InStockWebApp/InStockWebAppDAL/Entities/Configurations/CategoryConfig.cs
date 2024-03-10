using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InStockWebAppDAL.Entities.Configurations;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category")
            .HasKey("Id");
        builder.Property(p => p.Name)
            .HasMaxLength(80);
        builder.Property(p => p.Description)
            .HasMaxLength(2000);
    }
}