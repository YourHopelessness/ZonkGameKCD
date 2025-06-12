using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Entites.Auth.View;
using ZonkGame.DB.Entites.Zonk;

namespace ZonkGame.DB.Context
{
    public class AuthContext(DbContextOptions<AuthContext> options) : DbContext(options)
    {
        public DbSet<ApplicationUser> ApplicationUser { get; set; } = default!;
        public DbSet<Permission> Permissions { get; set; } = default!;
        public DbSet<Role> Role { get; set; } = default!;
        public DbSet<RolePermission> RolePermissions { get; set; } = default!;
        public DbSet<UserRole> UserRoles { get; set; } = default!;
        public DbSet<ApiResource> ApiResource { get; set; } = default!;
        public DbSet<ApiResourcePermission> ApiResourcePermissions { get; set; } = default!;
        public DbSet<ResourcePermissionView> ResourcePermissionView { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.UseOpenIddict();

            #region Navigation
            builder.Entity<Role>()
                   .HasMany(x => x.RolePermissions)
                   .WithOne(x => x.Role)
                   .HasForeignKey("role_id")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Role>()
                   .HasMany(x => x.UserRole)
                   .WithOne(x => x.Role)
                   .HasForeignKey("role_id")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Permission>()
                   .HasMany(x => x.RolePermissions)
                   .WithOne(x => x.Permission)
                   .HasForeignKey("permission_id")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Permission>()
                   .HasMany(x => x.ResourcePermissions)
                   .WithOne(x => x.Permission)
                   .HasForeignKey("permission_id")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                   .HasMany(x => x.UserRole)
                   .WithOne(x => x.User)
                   .HasForeignKey("user_id")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApiResource>()
                   .HasMany(x => x.ResourcePermissions)
                   .WithOne(x => x.ApiResource)
                   .HasForeignKey("api_resource_id")
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Date and Time
            builder.Entity<Role>()
                   .Property(g => g.CreatedAt)
                   .HasConversion<DateTime>()
                   .HasDefaultValueSql("timezone('utc', now())");

            builder.Entity<ApplicationUser>()
                   .Property(g => g.CreatedAt)
                   .HasConversion<DateTime>()
                   .HasDefaultValueSql("timezone('utc', now())");

            builder.Entity<ApplicationUser>()
                   .Property(g => g.LastLogin)
                   .HasConversion<DateTime>()
                   .HasDefaultValueSql("timezone('utc', now())");

            builder.Entity<Permission>()
                   .Property(g => g.CreatedAt)
                   .HasConversion<DateTime>()
                   .HasDefaultValueSql("timezone('utc', now())");
            #endregion

            builder.Entity<ApiResource>()
                   .Property(g => g.ApiName)
                   .HasConversion<string>();

            builder.Entity<ResourcePermissionView>()
                   .HasNoKey()
                   .ToView("resource_permission_view");
        }
    }
}
