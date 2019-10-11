using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Telemedicine.API.Models
{
    public class Role : IdentityRole<int>
    {
        public UserRole UserRole { get; set; }
    }
}