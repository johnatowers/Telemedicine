using System.Threading.Tasks;
using Telemedicine.API.Models;
using System.Collections.Generic;
using Telemedicine.API.Helpers;
using System.Linq;
using System; 


namespace Telemedicine.API.Data
{
    public interface ITelemedRepository
    {
         void Add<T>(T entity) where T: class;

         void Delete<T>(T entity) where T: class;

         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> getUser(int id);

         Task <Document> GetDocument(int id); 

        // Use to check if a select already exists
         Task<Select> GetSelect(int userId, int recipientId);

         Task<Message> GetMessage(int id);

        // Inbox, outbox, or unread messages
         Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);

         Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);

        //video 111
        //  Task <Photo> GetMainPhotoForUser(int userId); 
    }
}