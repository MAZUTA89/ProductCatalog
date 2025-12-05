using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.Core.Entities;

namespace ProductCatalog.Infrastructure.Data.AppDbContext.EntityTypeConfigurations
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id).HasName("ProductId_PRIMARY_KEY_CONSTRAINT");

            builder.Property(p => p.Id).HasColumnName("ProductId")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(p => p.Title).HasColumnName("ProductTitle")
                .IsRequired();

            builder.HasAlternateKey(p => p.Title);

            builder.Property(p => p.Description).IsRequired(false);

            builder.HasMany(p => p.Images)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
