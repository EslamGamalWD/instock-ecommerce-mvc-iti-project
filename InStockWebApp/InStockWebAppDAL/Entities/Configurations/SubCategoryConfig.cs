using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InStockWebAppDAL.Entities.Configurations;

public class SubCategoryConfig : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.ToTable("SubCategory")
            .HasKey("Id");
        builder.Property(p => p.Name)
            .HasMaxLength(100);
        builder.Property(p => p.Description)
            .HasMaxLength(250);
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
    }
}