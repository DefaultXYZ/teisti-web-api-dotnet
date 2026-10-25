using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teisti.Domain.Models;

namespace Teisti.Infrastructure.Database.Configurations;

public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);
        builder.Property(product => product.Name)
            .IsRequired();
        builder.Property(product => product.Price)
            .IsRequired()
            .HasPrecision(18, 2);
    }
}
