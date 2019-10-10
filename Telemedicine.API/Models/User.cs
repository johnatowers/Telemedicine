using System;
namespace Telemedicine.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; }

        public string firstName { get; set; }

        public string middleName { get; set; }

        public string lastName { get; set; }

        public string suffix {get; set; }

        // Section 9 properties
        public string Gender { get; set; }

        public DateTime DateofBirth { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        // When registering a user of type patient or admin, we'll enter default value null
        public string DeaId { get; set; }

        // When registering a user of type patient or admin, we'll enter default value null
        public string TypeOfDoctor { get; set; }

        // When registering a user of type doctor or admin, we'll enter default value null
        public string Notes { get; set; }

        // When registering a user of type doctor or admin, we'll enter default value null
        public string healthConditions { get; set; }

        // When registering a user of type doctor or admin, we'll enter default value null
        public string Allergies { get; set; }

        // When registering a user of type doctor or admin, we'll enter default value null
        public string Medications { get; set; }
    }
}