namespace Telemedicine.API.Dtos
{

    /*
            id: number;
    username: string;
    firstName: string;
    middleName: string;
    lastName: string;
    suffix: string;
    age: string;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    address: string;
    healthConditions: string;
    allergies: string;
    medications: string;
    city: string;
    country: string;
    interests?: string;
    introduction?: string;
    lookingFor?: string;
    photos?: Photo[];
     */
    public class UserForUpdateDto
    {
        public string HealthConditions { get; set; }
        public string Allergies { get; set; }
        public string Medications { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}