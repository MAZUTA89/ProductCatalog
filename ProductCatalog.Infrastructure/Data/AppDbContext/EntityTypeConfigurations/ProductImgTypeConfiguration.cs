using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.Core.Entities;

namespace ProductCatalog.Infrastructure.Data.AppDbContext.EntityTypeConfigurations
{
    public class ProductImgTypeConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(i => i.Id).HasName("CONSTRAINT_PRIMARY_KEY_ImageId");
            builder.Property(i => i.Id).HasColumnName("ImageId");

            builder.HasAlternateKey(i => i.FileName);
        }
    }
}
