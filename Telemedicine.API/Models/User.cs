using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
namespace Telemedicine.API.Models
{
    public class User : IdentityUser<int>
    {

        public string Role { get; set; }

        // When registering a patient (not doctor), we'll enter default value 0
        public string DeaId { get; set; }

         // Section 9 properties
        public string Gender { get; set; }

        public DateTime DateofBirth { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}