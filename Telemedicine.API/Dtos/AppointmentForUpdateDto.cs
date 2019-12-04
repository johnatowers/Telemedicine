using System;

namespace Telemedicine.API.Dtos
{
    public class AppointmentForUpdateDto
    {
        public string Title { get; set; }

        public string PrimaryColor { get; set; }

        public string SecondaryColor { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}