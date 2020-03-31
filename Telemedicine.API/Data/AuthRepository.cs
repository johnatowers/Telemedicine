using Microsoft.EntityFrameworkCore;
 using System.Threading.Tasks;
 using Telemedicine.API.Models;
 using System.Linq; 
 using System; 

namespace Telemedicine.API.Data
{
    // This class is no longer implemented anywhere
    public class AuthRepository : IAuthRepository
    {
         
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        // registering user
         public async Task<User> Register(User user, string password, string deaId) 
         {
             byte[] passwordHash, passwordSalt;
             CreatePasswordHash(password, out passwordHash, out passwordSalt);
             
             //user.PasswordHash = passwordHash;
             //user.PasswordSalt = passwordSalt;
             user.DeaId = deaId;

            //TODO: this is most likely where we will query database of DEA IDs to check this ID is valid
            // We can create a method similar to VerifyPasswordHash and UserExists
            // For now, I'll just make sure it does not equal 0

             await _context.Users.AddAsync(user);
             await _context.SaveChangesAsync();

             return user;
         }

         private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
         { 
             using(var hmac = new System.Security.Cryptography.HMACSHA512()) 
             {
                 passwordSalt = hmac.Key;
                 passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
             }
             
         }

         // logging in to api
         public async Task<User> Login(string username, string password) 
         {
             var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

             if (user == null)
                return null;

            //if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            //    return null;

            return user;
         }

         private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
         { 
             using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) 
             {
                 var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                 for (int i = 0; i < computedHash.Length; i++) {
                     if (computedHash[i] != passwordHash[i]) return false;
                 }
             }
             return true;
         }

         // check if user exists or if a user already registered with a DEA ID
         public async Task<bool> UserExists(string username) 
         {
             if (await _context.Users.AnyAsync(x => x.UserName == username))
                return true;

            return false;
         }

        // Not currently used
         public async Task<bool> DoctorExists(string deaId)
         {
             // First checks that deaId is not "0"
             // if it is 0, that indicates there is no ID, so we don't need to check
             // if it has already been registered
            if (!(deaId.Length > 1 && !deaId.Equals("0")))
                return false;

            if (await _context.Users.AnyAsync(y => y.DeaId == deaId))
                return true;
            
            return false;
         } 

         /*public async Task<Photo> GetPhoto(int id){
             var photo = await _context.Photo.FirstOrDefaultAsync(p => p.Id == id);

             return photo; 
         } */
    }
}