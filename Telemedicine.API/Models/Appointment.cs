using System;

namespace Telemedicine.API.Models
{
    public class Appointment
    {
        public int Id {get; set;}

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public User Patient { get; set; }

        public User Doctor { get; set; }

        public string Title { get; set; }

        public string PrimaryColor { get; set; }

        public string SecondaryColor { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}