using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Models;

namespace Telemedicine.API.Data
{
    public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, 
    UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options){}
        
        public DbSet<Value> Values { get; set; }
        public DbSet<Document> Documents { get; set;}

        //public DbSet<User> Users { get; set; }

        public DbSet<Relationship> Relationships { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserRole>(userRole =>
        {
            userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

            userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();


            userRole.HasOne<User>(ur => ur.User)
            .WithOne(r => r.UserRole)
            .HasForeignKey<UserRole>(ur => ur.UserId)
            .IsRequired();
        });

        //builder.Entity<UserRole>(userRole =>
        //{
        //    userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

        //    userRole.HasOne(ur => ur.Role)
        //    .WithOne(r => r.UserRole)
        //    .HasForeignKey<UserRole>(ur => ur.RoleId)
        //    .IsRequired();

        //    userRole.HasOne(ur => ur.User)
        //    .WithOne(r => r.UserRole)
        //    .HasForeignKey<UserRole>(ur => ur.UserId)
        //    .IsRequired();
        //});

        builder.Entity<Relationship>()
            .HasKey(k => new { k.PatientId, k.DoctorId});

        builder.Entity<Relationship>()
            .HasOne(u => u.Doctor)
            .WithMany(u => u.Patient)
            .HasForeignKey(u => u.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.Entity<Relationship>()
            .HasOne(u => u.Patient)
            .WithMany(u => u.Doctor)
            .HasForeignKey(u => u.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

    }
    }

}