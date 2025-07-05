using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.ApiService.Data.Mappings;

public class DeveloperMapping : IEntityTypeConfiguration<Developer>
{
    public void Configure(EntityTypeBuilder<Developer> builder)
    {
        builder.ToTable("Developer");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityByDefaultColumn();

        builder.Property(p => p.Name)
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(p => p.DevLevel)
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(p => p.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(p => p.Technologies)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.ExpirienceTime)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasDefaultValue(DateTime.Now.ToUnspecifiedKind())
            .HasColumnType("timestamp");

        builder.Property(x => x.UpdatedAt)
            .HasDefaultValue(DateTime.Now.ToUnspecifiedKind())
            .HasColumnType("timestamp");

        builder.HasMany(p => p.Projects)
            .WithMany(p => p.Developers)
            .UsingEntity<Dictionary<string, object>>(
                "ProjectDeveloper",
                j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                j => j.HasOne<Developer>().WithMany().HasForeignKey("DeveloperId"),
                j =>
                {
                    j.HasKey("ProjectId", "DeveloperId");
                });
    }
}