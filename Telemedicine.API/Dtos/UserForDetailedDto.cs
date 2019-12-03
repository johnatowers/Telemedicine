using System;
using System.Collections.Generic;

namespace Telemedicine.API.Dtos
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string firstName { get; set; }

        public string middleName { get; set; }

        public string lastName { get; set; }

        public string suffix {get; set; }

        // When registering a patient (not doctor), we'll enter default value 0
        //public string DeaId { get; set; }

         // Section 9 properties
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string DeaId { get; set; }

        public string healthConditions { get; set; }
        public string allergies { get; set; }
        public string medications { get; set; }
        public ICollection<DocumentForDetailedDto> Documents { get; set; }
    }
}