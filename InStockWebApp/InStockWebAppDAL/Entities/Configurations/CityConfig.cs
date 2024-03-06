using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InStockWebAppDAL.Entities.Configurations;

public class CityConfig : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("City")
            .HasKey("Id");
        builder.Property(p => p.Name)
            .HasMaxLength(100);
    }
}