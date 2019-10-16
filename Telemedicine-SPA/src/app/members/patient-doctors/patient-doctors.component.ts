import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';


@Component({
  selector: 'app-patient-doctors',
  templateUrl: './patient-doctors.component.html',
  styleUrls: ['./patient-doctors.component.css']
})
export class PatientDoctorsComponent implements OnInit {
  users: User[];
  pagination: Pagination;

  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

   loadUsers() {
     this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe((res: PaginatedResult<User[]>) => {
       this.users = res.result;
       this.pagination = res.pagination;
     }, error => {
       this.alertify.error(error);
     });
   }

  // This method is custom and lets us filter out entries on the user table without
  // creating emtpy divs on a page. Change x.gender to a different field to change filter.
  // NOTE: this worked fine before video 91.
  filterItemsOfType(type: string) {
    return this.users.filter(x => x.gender === type);
  }
}
