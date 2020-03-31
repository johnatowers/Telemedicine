import { User } from './user';

export interface Appointment {
    // public int Id {get; set;}
    id: number;
    //     public int PatientId { get; set; }
    patientId: number;
    //     public int DoctorId { get; set; }
    doctorId: number;
    //     public string PatientFirstName { get; set; }
    patientFirstName: string;
    //     public string PatientLastName { get; set; }
    patientLastName: string;
    //     public string DoctorFirstName { get; set; }
    doctorFirstName: string;
    //     public string DoctorLastName { get; set; }
    doctorLastName: string;
    //     public string Title { get; set; }
    title: string;
    //     public string PrimaryColor { get; set; }
    primaryColor: string;
    //     public string SecondaryColor { get; set; }
    secondaryColor: string;
    //     public DateTime StartDate { get; set; }
    startDate: Date;
    //     public DateTime EndDate { get; set; }
    endDate: Date;
}
