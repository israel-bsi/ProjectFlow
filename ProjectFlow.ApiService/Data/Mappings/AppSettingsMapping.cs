using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.ApiService.Data.Mappings;

public class AppSettingsMapping : IEntityTypeConfiguration<AppSettings>
{
    public void Configure(EntityTypeBuilder<AppSettings> builder)
    {
        builder.ToTable("AppSettings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ValuePerHour)
            .HasColumnType("money");

        builder.Property(x => x.DaysToAddOnFinishedDate)
            .HasColumnType("integer");

        builder.HasData(new AppSettings
        {
            Id = 1,
            ValuePerHour = 150,
            DaysToAddOnFinishedDate = 15
        });
    }
}