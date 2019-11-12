using System;
using Telemedicine.API.Models;

namespace Telemedicine.API.Dtos
{
    public class AppointmentToReturnDto
    {
        public int Id {get; set;}

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public string PatientFirstName { get; set; }

        public string PatientLastName { get; set; }

        public string DoctorFirstName { get; set; }

        public string DoctorLastName { get; set; }

        public string Title { get; set; }

        public string PrimaryColor { get; set; }

        public string SecondaryColor { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}