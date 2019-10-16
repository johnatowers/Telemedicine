namespace Telemedicine.API.Models
{
    public class Relationship
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public User Patient { get; set; }
        public User Doctor { get; set; }
    }
}