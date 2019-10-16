using System.Collections.Generic;
using System.Threading.Tasks;
using Telemedicine.API.Models;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Helpers; 
using System.Linq; 
using System; 

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

        //video 111 
        // public async Task<Photo> GetMainPhotoForUser(int userId){
        //     return await _context.Documents.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain); 
        // }

        public async Task<Document> GetDocument(int id)
        {
            var photo = await _context.Documents.FirstOrDefaultAsync(p => p.id == id);
            return photo;  
        }

        public async Task<User> getUser(int id)
        {
            var user = await _context.Users.Include(p => p.Documents).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.Documents).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            // return true if positive # of changes have been done
            return await _context.SaveChangesAsync() > 0;
        }
    }
}