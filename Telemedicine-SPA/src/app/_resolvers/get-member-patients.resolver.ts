import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Appointment } from '../_models/Appointment';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class GetMemberPatientsResolver implements Resolve<Appointment[]> {
    pageNumber = 1;
    pageSize = 15;
    constructor(private userService: UserService,
                private router: Router, private alertify: AlertifyService, private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Appointment[]> {
        return this.userService.getAppointments(this.authService.decodedToken.nameid).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving appointments');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
