 using System.Threading.Tasks;
 using Telemedicine.API.Models;

 namespace Telemedicine.API.Data
{
    public interface IAuthRepository
    {
          // registering user
         Task<User> Register(User user, string password, string deaId);

         // logging in to api
         Task<User> Login(string username, string password);

         // check if user exists
         Task<bool> UserExists(string username);

        // check if doctor has already registered with a specific DEA Id
        Task<bool> DoctorExists(string deaId); 

        //Task <Photo> GetPhoto(int id); 

    }
} 