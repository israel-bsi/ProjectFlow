using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFlow.Core.Enums;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.ApiService.Data.Mappings;

public class ProjectMapping : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Project");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityByDefaultColumn()
            .HasIdentityOptions(startValue: 76);

        builder.Property(p => p.Title)
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(p => p.TotalHours)
            .IsRequired()
            .HasColumnType("smallint");

        builder.Property(p => p.Requester)
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(p => p.TotalValue)
            .HasColumnType("money")
            .IsRequired();

        builder.Property(p => p.DiscountValue)
            .HasColumnType("money")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(p => p.DiscountType)
            .HasColumnType("smallint")
            .HasDefaultValue(EDiscountType.None)
            .IsRequired();

        builder.Property(p => p.Deadline)
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(p => p.DaysToAddToDeadline)
            .HasColumnType("smallint")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(p => p.ProjectStatus)
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(p => p.PaymentStatus)
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(x => x.RequestedAt)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(x => x.DevelopmentStart)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(x => x.DevelopmentEnd)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(x => x.ValidationStart)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(x => x.ValidationEnd)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(x => x.FinishedIn)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(p => p.IsActive)
            .HasColumnType("boolean")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValue(DateTime.Now.ToUnspecifiedKind())
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValue(DateTime.Now.ToUnspecifiedKind())
            .IsRequired();
    }
}