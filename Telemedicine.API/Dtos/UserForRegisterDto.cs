using System;
using System.ComponentModel.DataAnnotations;
using Telemedicine.API.Models;

namespace Telemedicine.API.Dtos
{
    public class UserForRegisterDto
    {
        //test
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 20 characteres")]
        public string Password { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string middleName { get; set; }

        [Required]
        public string lastName { get; set; }

        public string suffix {get; set; }

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

        // When registering a user of type patient or admin, we'll enter default value null
        public string DeaId { get; set; }

        // When registering a user of type patient or admin, we'll enter default value null
        public string TypeOfDoctor { get; set; }


        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
            suffix = null;
            DeaId = null;
            TypeOfDoctor = null;

        }        
    }
}