using System.Collections.Generic;
using Hermes.API.Advertisement.Domain.Entities;
using Hermes.API.Advertisement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.API.Advertisement.Domain.EntityConfigurations
{
    public class AdvertisementApplicationEntityConfiguration : IEntityTypeConfiguration<AdvertisementApplication>
    {
        public void Configure(EntityTypeBuilder<AdvertisementApplication> builder)
        {
            builder.Property(p => p.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(p => p.EstimatedBarrowDays)
                .IsRequired();

            builder.Property(p => p.ApplicantId)
                .IsRequired();

            builder.Property(p => p.AdvertisementId)
                .IsRequired();

            builder.Property(p => p.IsDeleted)
                .HasDefaultValueSql("0")
                .IsRequired();

            builder.Property(p => p.StatusId)
                .HasDefaultValue(AdvertisementApplicationStatuses.Created)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("getutcdate()")
                .IsRequired();
        }
    }
}