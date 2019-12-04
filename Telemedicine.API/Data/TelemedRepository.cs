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

        public async Task<Select> GetSelect(int userId, int recipientId)
        {
            // Where userId matches the SelectorId and recipientId matches SelecteeId
            // If False it returns null, if True it will return the 'select' 
            return await _context.Relationships.FirstOrDefaultAsync( u =>
            u.SelectorId == userId && u.SelecteeId == recipientId);
        }

        public async Task<User> getUser(int id)
        {
            var user = await _context.Users.Include(p => p.Documents).Include(p => p.UserRole).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.AsQueryable();

            if (userParams.Selectors) {
                var userSelectors = await GetUserRelationships(userParams.UserId, userParams.Selectors);
                users = users.Where(u => userSelectors.Contains(u.Id));
            }
            if (userParams.Selectees) {
                var userSelectees = await GetUserRelationships(userParams.UserId, userParams.Selectors);
                users = users.Where(u => userSelectees.Contains(u.Id));
            }
            // returns only doctors
            if (userParams.DoctorRoleOnly) {
                 users = users.Where(u => u.UserRole.RoleId == 3);
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        private async Task<IEnumerable<int>> GetUserRelationships(int id, bool selectors) {
            var user = await _context.Users
            .Include(x => x.Selectors)
            .Include(x => x.Selectees)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (selectors)
            {
                return user.Selectors.Where(u => u.SelecteeId == id).Select(i => i.SelectorId);
            }
            else {
                return user.Selectees.Where(u => u.SelectorId == id).Select(i => i.SelecteeId);
            }
        }

        public async Task<bool> SaveAll()
        {
            // return true if positive # of changes have been done
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages.Include(u => u.Sender) //.ThenInclude(p => p.Photos)
            .Include(u => u.Recipient) //.ThenInclude(p => p.Photos)
            .AsQueryable();

            // find inbox or outbox messages
            switch (messageParams.MessageContainer) {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.IsRead == false
                        && u.RecipientDeleted == false);
                    break;
            }

            messages = messages.OrderBy(d => d.MessageSent);
            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            // Get complete conversation between 2 users
            var messages = await _context.Messages.Include(u => u.Sender) //.ThenInclude(p => p.Photos)
            .Include(u => u.Recipient) //.ThenInclude(p => p.Photos)
            .Where(m => m.RecipientId == userId && m.RecipientDeleted == false 
                && m.SenderId == recipientId 
                || m.RecipientId == recipientId && m.SenderId == userId 
                && m.SenderDeleted == false)
                .OrderBy(m => m.MessageSent)
                .ToListAsync();
            
            return messages;
        }

        public async Task<Appointment> GetAppointment(int id)
        {
            return await _context.Appointments.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Appointment>> GetAppointmentsForUser(UserParams userParams)
        {
            var appointments = _context.Appointments.Include(u => u.Patient) //.ThenInclude(p => p.Photos)
            .Include(u => u.Doctor) //.ThenInclude(p => p.Photos)
            .AsQueryable();

            appointments = appointments.Where(u => u.PatientId == userParams.UserId || u.DoctorId == userParams.UserId);
            return await PagedList<Appointment>.CreateAsync(appointments, userParams.PageNumber, userParams.PageSize);
        }
    }
}