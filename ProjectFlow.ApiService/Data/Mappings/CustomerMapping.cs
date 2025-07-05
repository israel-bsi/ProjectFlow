using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.ApiService.Data.Mappings;

public class CustomerMapping : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Name)
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(x => x.BusinessName)
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(c => c.DocumentNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.PersonType)
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(c => c.Phone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.CellPhone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValue(DateTime.Now.ToUnspecifiedKind())
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValue(DateTime.Now.ToUnspecifiedKind())
            .IsRequired();

        builder.HasMany(x => x.Projects)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}