import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PatientChartComponent } from './patient-chart/patient-chart.component';
import { PatientMessagesComponent } from './patient-messages/patient-messages.component';
import { PatientAppointmentsComponent } from './patient-appointments/patient-appointments.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'patient-chart', component: PatientChartComponent},
            { path: 'patient-messages', component: PatientMessagesComponent},
            { path: 'patient-appointments', component: PatientAppointmentsComponent},
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full'},
];
