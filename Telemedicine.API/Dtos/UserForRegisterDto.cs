using System.ComponentModel.DataAnnotations;
using System;

namespace Telemedicine.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 20 characteres")]
        public string Password { get; set; }

        // When registering a patient (not doctor), we'll enter default value 0
        public string DeaId { get; set; }

        public string Role { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        //[Required]
        // Not required because some countries don't have states in their addresses
        // i.e. London, England 
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive {get; set;}

        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
            DeaId = "0";
            Role = "User";
        }        
    }
}