using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Telemedicine.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Telemedicine.API.Data
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                // create some roles
                var roles = new List<Role>
                {
                    new Role{Name = "Patient"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Doctor"}

                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    userManager.CreateAsync(user, "password").Wait();
                    userManager.AddToRoleAsync(user, "Patient");
                }

                // create admin user
                var adminUser = new User
                {
                    UserName = "Admin"
                };

                var result = userManager.CreateAsync(adminUser, "admin").Result;

                if (result.Succeeded) {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    //userManager.AddToRolesAsync(admin, new[] {"Admin"});
                    userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
         { 
             using(var hmac = new System.Security.Cryptography.HMACSHA512()) 
             {
                 passwordSalt = hmac.Key;
                 passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
             }
             
         }
    }
}