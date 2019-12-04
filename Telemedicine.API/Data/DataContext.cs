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
        public DbSet<Document> Documents {get; set;}

        public DbSet<Select> Relationships {get; set;}

        public DbSet<Message> Messages { get; set; }

        public new DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        //public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Select>()
        .HasKey(k => new {k.SelectorId, k.SelecteeId});

        builder.Entity<Select>()
        .HasOne(u => u.Selectee)
        .WithMany(u => u.Selectors)
        .HasForeignKey(u => u.SelecteeId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Select>()
        .HasOne(u => u.Selector)
        .WithMany(u => u.Selectees)
        .HasForeignKey(u => u.SelectorId)
        .OnDelete(DeleteBehavior.Restrict);

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

        builder.Entity<Message>()
        .HasOne(u => u.Sender)
        .WithMany(m => m.MessagesSent)
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
        .HasOne(u => u.Recipient)
        .WithMany(m => m.MessagesReceived)
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Appointment>()
        .HasOne(u => u.Patient)
        .WithMany(a => a.PatientAppointments)
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Appointment>()
        .HasOne(u => u.Doctor)
        .WithMany(a => a.DoctorAppointments)
        .OnDelete(DeleteBehavior.Restrict);
    }
    }

}