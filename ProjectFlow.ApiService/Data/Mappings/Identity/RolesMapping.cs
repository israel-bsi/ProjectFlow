using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectFlow.ApiService.Data.Mappings.Identity;

public class RolesMapping : IEntityTypeConfiguration<IdentityRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.NormalizedName).IsUnique();

        builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
        builder.Property(u => u.Name).HasMaxLength(256);
        builder.Property(u => u.NormalizedName).HasMaxLength(256);

        builder.HasData(new IdentityRole<int>
        {
            Id = 2,
            Name = Configuration.Roles.User,
            NormalizedName = Configuration.Roles.User.ToUpper()
        });
    }
}