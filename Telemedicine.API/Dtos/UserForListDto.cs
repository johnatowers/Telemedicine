using System;
namespace Telemedicine.API.Dtos
{
    public class UserForListDto
    {
        public int Id { get; set; }

        public string firstName { get; set; }

        public string middleName { get; set; }

        public string lastName { get; set; }

        public string suffix {get; set; }

        public string Username { get; set; }

        // When registering a patient (not doctor), we'll enter default value 0
        //public string DeaId { get; set; }

         // Section 9 properties
        public string Gender { get; set; }

        public int Age { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string PhotoUrl { get; set; }

    }
}