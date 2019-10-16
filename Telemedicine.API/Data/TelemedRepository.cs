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

        public async Task<Relationship> GetRelationship(int userId, int recipientId)
        {
            return await _context.Relationships.FirstOrDefaultAsync(u => 
            u.PatientId == userId && u.DoctorId == recipientId);
        }

        public async Task<User> getUser(int id)
        {
            var user = await _context.Users.Include(p => p.Documents).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.AsQueryable();

            if (userParams.Patients)
            {
                var userPatients = await GetUserRelationships(userParams.UserId, userParams.Patients);
                users = users.Where(u => userPatients.Contains(u.Id));
            }
            
            if (userParams.Doctors)
            {
                var userDoctors = await GetUserRelationships(userParams.UserId, userParams.Doctors);
                users = users.Where(u => userDoctors.Contains(u.Id));

            }
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        private async Task<IEnumerable<int>> GetUserRelationships(int id, bool partners)
        {
            var user = await _context.Users
            .Include(x => x.Patient)
            .Include(x => x.Doctor)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (partners)
            {
                return user.Patient.Where(u => u.DoctorId == id).Select(i => i.PatientId);
            }
            else 
            {
                return user.Doctor.Where(u => u.PatientId == id).Select(i => i.DoctorId);
            }
        }

        public async Task<bool> SaveAll()
        {
            // return true if positive # of changes have been done
            return await _context.SaveChangesAsync() > 0;
        }
    }
}