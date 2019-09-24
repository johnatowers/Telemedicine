using System.Collections.Generic;
using System.Threading.Tasks;
using Telemedicine.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Telemedicine.API.Data
{
    public class TelemedRepository : ITelemedRepository
    {

        private readonly DataContext _context;

        public TelemedRepository(DataContext context) 
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> getUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            // return true if positive # of changes have been done
            return await _context.SaveChangesAsync() > 0;
        }
    }
}