import { Document } from './Document';


// TODO: This should match DTO's and DB scheme for the user table
export interface User {
    id: number;
    username: string;
    deaId: string;
    role: string;
    age: string;
    gender: string;
    dateOfBirth: Date;
    created: Date;
    lastActive: Date;
    documentUrl: string;
    address: string;
    healthConditions: string;
    allergies: string;
    medications: string;
    city: string;
    country: string;
    documents?: Document[];
}
