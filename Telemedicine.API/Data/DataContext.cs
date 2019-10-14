using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Models;

namespace Telemedicine.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options){}
        
        public DbSet<Value> Values { get; set; }


        public DbSet<User> Users { get; set; }

        public DbSet<Photo> Documents {get; set;}

    }
}