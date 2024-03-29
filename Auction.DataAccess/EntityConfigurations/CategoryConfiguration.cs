﻿using Auction.DataAccess.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Auction.DataAccess.EntityConfigurations
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            ToTable("Categories");

            Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
