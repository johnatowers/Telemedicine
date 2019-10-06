import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-patient-doctors',
  templateUrl: './patient-doctors.component.html',
  styleUrls: ['./patient-doctors.component.css']
})
export class PatientDoctorsComponent implements OnInit {
  users: User[];

  constructor(private userService: UserService, private alertify: AlertifyService, 
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'];
    });
  }

  // loadUsers() {
  //   this.userService.getUsers().subscribe((users: User[]) => {
  //     this.users = users;
  //   }, error => {
  //     this.alertify.error(error);
  //   });
  // }

  // This method is custom and lets us filter out entries on the user table without
  // creating emtpy divs on a page. Change x.gender to a different field to change filter.
  // NOTE: this worked fine before video 91.
  filterItemsOfType(type: string) {
    return this.users.filter(x => x.gender === type);
  }
}
