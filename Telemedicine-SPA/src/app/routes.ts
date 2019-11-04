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
import { PatientSelectorsComponent } from './members/patient-selectors/patient-selectors.component';
import { PatientSelectorsResolver } from './_resolvers/PatientSelectors.resolver';
import { PatientSelecteesComponent } from './members/patient-selectees/patient-selectees.component';
import { PatientSelecteesResolver } from './_resolvers/PatientSelectees.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { DoctorPatientsComponent } from './members/doctor-patients/doctor-patients.component';
import { DoctorSelectorsComponent } from './members/doctor-selectors/doctor-selectors.component';
import { DoctorSelecteesComponent } from './members/doctor-selectees/doctor-selectees.component';
import { NavComponent } from './nav/nav.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'patient-chart', component: PatientChartComponent,
                resolve: { user: MemberEditResolver}},
            { path: 'patient-messages', component: PatientMessagesComponent, resolve: {messages: MessagesResolver}},
            { path: 'patient-appointments', component: PatientAppointmentsComponent},
            { path: 'patient-doctors', component: PatientDoctorsComponent, resolve: { users: PatientDoctorsResolver}},
            { path: 'members/:id', component: MemberDetailComponent,
                resolve: { user: MemberDetailResolver}},
            { path: 'member/edit', component: MemberEditComponent,
                resolve: { user: MemberEditResolver}, canDeactivate: [PreventUnsavedChanges]},
            {path: 'admin', component: AdminPanelComponent, data: {roles: ['Admin']}},
            {path: 'patient-selectors', component: PatientSelectorsComponent, resolve: {users: PatientSelectorsResolver}},
            {path: 'patient-selectees', component: PatientSelecteesComponent, resolve: {users: PatientSelecteesResolver}},
            {path: 'doctor-patients', component: DoctorPatientsComponent, resolve: {users: PatientDoctorsResolver}},
            {path: 'doctor-selectors', component: DoctorSelectorsComponent, resolve: {users: PatientSelectorsResolver}},
            {path: 'doctor-selectees', component: DoctorSelecteesComponent, resolve: {users: PatientSelecteesResolver}},
            {path: 'nav', component: NavComponent, resolve: {user: MemberEditResolver}}
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full'},
];
