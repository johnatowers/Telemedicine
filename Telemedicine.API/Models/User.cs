namespace Telemedicine.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        // When registering a patient (not doctor), we'll enter default value 0
        public string DeaId { get; set; }

        // 0 = patient, 1 = doctor, 2 = admin
        public int role { get; set; }
    }
}