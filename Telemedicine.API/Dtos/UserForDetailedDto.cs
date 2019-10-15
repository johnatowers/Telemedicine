using System;
using System.Collections.Generic;

namespace Telemedicine.API.Dtos
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }

        // When registering a patient (not doctor), we'll enter default value 0
        //public string DeaId { get; set; }

         // Section 9 properties
        public string Gender { get; set; }

        public int Age { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public ICollection<PhotosForDetailedDto> Documents { get; set; }
    }
}