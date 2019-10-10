using System.ComponentModel.DataAnnotations;

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

        //[Required]
        // When registering a patient (not doctor), we'll enter default value 0
        public string DeaId { get; set; }
    }
}