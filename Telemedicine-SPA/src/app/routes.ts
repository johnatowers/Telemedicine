import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PatientChartComponent } from './patient-chart/patient-chart.component';
import { PatientMessagesComponent } from './patient-messages/patient-messages.component';
import { PatientAppointmentsComponent } from './patient-appointments/patient-appointments.component';
import { PatientDoctorsComponent } from './members/patient-doctors/patient-doctors.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { PatientDoctorsResolver } from './_resolvers/patient-doctors.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { PatientAppointmentsResolver } from './_resolvers/patient-appointments.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'patient-chart', component: PatientChartComponent},
            { path: 'patient-messages', component: PatientMessagesComponent},
            { path: 'patient-appointments', component: PatientAppointmentsComponent,
                resolve: { user: PatientAppointmentsResolver}},
            { path: 'patient-doctors', component: PatientDoctorsComponent,
                resolve: { users: PatientDoctorsResolver}},
            { path: 'members/:id', component: MemberDetailComponent,
                resolve: { user: MemberDetailResolver}},
            { path: 'member/edit', component: MemberEditComponent,
                resolve: { user: MemberEditResolver}, canDeactivate: [PreventUnsavedChanges]},
            {path: 'admin', component: AdminPanelComponent, data: {roles: ['Admin']}}
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full'},
];
