using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.ApiService.Data.Mappings;

public class ProjectServiceMapping : IEntityTypeConfiguration<ProjectService>
{
    public void Configure(EntityTypeBuilder<ProjectService> builder)
    {
        builder.ToTable("ProjectService");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.Hours)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasColumnType("money")
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValue(DateTime.Now.ToUnspecifiedKind())
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValue(DateTime.Now.ToUnspecifiedKind())
            .IsRequired();

        builder.HasOne(x => x.Project)
            .WithMany(x => x.ProjectServices)
            .HasForeignKey(x => x.ProjectId)
            .IsRequired();
    }
}