using System;
using System.ComponentModel.DataAnnotations;

namespace Telemedicine.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 20 characteres")]
        public string Password { get; set; }

        // This will be removed soon
        public string Role { get; set; }

        // WHEN UPDATING ANGULAR FRONT END TO INCLUDE THESE VALUES
        // ADD [Required] ABOVE THE ONES THAT ARE ENTERED IN THE UI

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

        public UserForRegisterDto()
        {
            // WHEN UPDATING ANGULAR FRONT END TO INCLUDE THESE VALUES
            // REMOVE THE FOLLOWING
            Gender = "NA";
            DateofBirth = DateTime.Now;
            Address = "NA";
            City = "NA";
            State = "NA";
            Country = "NA";
            // These will remain no matter what
            Role = "user";
            DeaId = "0";
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}