import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: User;

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  sendSelect(id: number) {
    this.userService.sendSelect(this.authService.decodedToken.nameid, id).subscribe(data => {
      this.alertify.success('You have selected: ' + this.user.username); // TODO - add User's first, middle, last to SPA/models/user
      // and display name here instead of username
    }, error => {
      this.alertify.error(error);
    });
      // Add to alertify: "would you like to schedule an appointment? " and have button that takes user to calendar
  }

}
