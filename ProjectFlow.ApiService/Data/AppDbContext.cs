using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.ApiService.Data;

public class AppDbContext :
    IdentityDbContext
    <
        User,
        IdentityRole<int>,
        int,
        IdentityUserClaim<int>,
        IdentityUserRole<int>,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>
    >
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Developer> Developers { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectService> ProjectServices { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<AppSettings> AppSettings { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        var hasher = new PasswordHasher<User>();
        
        CreateAdmin(modelBuilder, hasher);
    }

    private void CreateAdmin(ModelBuilder modelBuilder, PasswordHasher<User> hasher)
    {
        var adminUser = new User
        {
            Id = 1,
            GivenName = "Admin",
            UserName = "admin@admin.com",
            NormalizedUserName = "ADMIN@ADMIN.COM",
            Email = "admin@admin.com",
            NormalizedEmail = "ADMIN@ADMIN.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin@123");
        modelBuilder.Entity<User>().HasData(adminUser);
        var adminRole = new IdentityRole<int>
        {
            Id = 1,
            Name = Configuration.Roles.Admin,
            NormalizedName = Configuration.Roles.Admin.ToUpper()
        };
        modelBuilder.Entity<IdentityRole<int>>().HasData(adminRole);
        modelBuilder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id
        });
    }
}