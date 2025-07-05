using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.ApiService.Data.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.NormalizedUserName).IsUnique();
        builder.HasIndex(u => u.NormalizedEmail).IsUnique();

        builder.Property(u => u.GivenName).HasMaxLength(100);
        builder.Property(u => u.Email).HasMaxLength(180);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(180);
        builder.Property(u => u.UserName).HasMaxLength(160);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(180);
        builder.Property(u => u.PhoneNumber).HasMaxLength(20);
        builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

        builder.HasMany<IdentityUserClaim<int>>()
            .WithOne()
            .HasForeignKey(u => u.UserId)
            .IsRequired();

        builder.HasMany<IdentityUserLogin<int>>()
            .WithOne()
            .HasForeignKey(u => u.UserId)
            .IsRequired();

        builder.HasMany<IdentityUserToken<int>>()
            .WithOne()
            .HasForeignKey(u => u.UserId)
            .IsRequired();

        builder.HasMany<IdentityUserRole<int>>()
            .WithOne()
            .HasForeignKey(u => u.UserId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValue(DateTime.Now.ToUnspecifiedKind())
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValue(DateTime.Now.ToUnspecifiedKind())
            .IsRequired();

        builder.HasMany(u=>u.Projects)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}