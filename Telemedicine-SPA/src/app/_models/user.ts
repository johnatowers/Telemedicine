import { Photo } from './Photo';


// TODO: This should match DTO's and DB scheme for the user table
export interface User {
    id: number;
    username: string;
    role: string;
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
}
