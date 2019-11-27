import { Component, ChangeDetectionStrategy, ViewChild, TemplateRef, OnInit } from '@angular/core';
import { startOfDay, endOfDay, subDays, addDays, endOfMonth, isSameDay, isSameMonth, addHours } from 'date-fns';
import { Subject } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CalendarEvent, CalendarEventAction, CalendarEventTimesChangedEvent, CalendarView} from 'angular-calendar';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { Appointment } from '../_models/Appointment';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

const colors: any = {
  red: {
    primary: '#ad2121',
    secondary: '#FAE3E3'
  },
  blue: {
    primary: '#1e90ff',
    secondary: '#D1E8FF'
  },
  yellow: {
    primary: '#e3bc08',
    secondary: '#FDF1BA'
  }
};

@Component({
  selector: 'app-patient-appointments',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './patient-appointments.component.html',
  styleUrls: ['./patient-appointments.component.css']
})
export class PatientAppointmentsComponent implements OnInit {

  @ViewChild('modalContent', { static: true }) modalContent: TemplateRef<any>;

  view: CalendarView = CalendarView.Month;

  CalendarView = CalendarView;

  viewDate: Date = new Date();

  user: User;

  doctors: User[];
  selectsParam: string;

  // Retrieve from back end
  appointments: Appointment[];
  pagination: Pagination;
  // Create appointment to add to DB
  newAppointment: any = {};
  selectedDoctor: number;
  // addedAppointment: Appointment;


  // This will be the part that connects to the DB
  modalData: {
    action: string;
    event: CalendarEvent;
  };

  actions: CalendarEventAction[] = [
    {
      label: '<i class="fa fa-fw fa-pencil"></i>',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.handleEvent('Edited', event);
      }
    },
    {
      label: '<i class="fa fa-fw fa-times"></i>',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.events = this.events.filter(iEvent => iEvent !== event);
        this.handleEvent('Deleted', event);
      }
    }
  ];

  refresh: Subject<any> = new Subject();

  newEvents: CalendarEvent[] = [];

  // let event of events
  events: CalendarEvent[] = [];
  // events: CalendarEvent[] = [{
  //   start: subDays(startOfDay(new Date()), 1),
  //   end: addDays(new Date(), 1),
  //   title: 'Begin 3 Day Fast For Blood Work',
  //   color: colors.red,
  //   actions: this.actions,
  //   allDay: true,
  //   resizable: {
  //     beforeStart: true,
  //     afterEnd: true
  //   },
  //   draggable: true
  // },
  // {
  //   start: subDays(endOfMonth(new Date()), 3),
  //   end: addDays(endOfMonth(new Date()), 3),
  //   title: 'Check with Doctor if any side effects present',
  //   color: colors.blue,
  //   allDay: true
  // },
  // {
  //   start: addHours(startOfDay(new Date()), 2),
  //   end: new Date(),
  //   title: 'Neurologist Appointment',
  //   color: colors.yellow,
  //   actions: this.actions,
  //   resizable: {
  //     beforeStart: true,
  //     afterEnd: true
  //   },
  //   draggable: true
  //   }];

  // {
  //   start: subDays(startOfDay(new Date()), 1),
  //   end: addDays(new Date(), 1),
  //   title: 'A 3 day event',
  //   color: colors.red,
  //   actions: this.actions,
  //   allDay: true,
  //   resizable: {
  //     beforeStart: true,
  //     afterEnd: true
  //   },
  //   draggable: true
  // },
  // {
  //   start: startOfDay(new Date()),
  //   title: 'An event with no end date',
  //   color: colors.yellow,
  //   actions: this.actions
  // },
  // {
  //   start: subDays(endOfMonth(new Date()), 3),
  //   end: addDays(endOfMonth(new Date()), 3),
  //   title: 'A long event that spans 2 months',
  //   color: colors.blue,
  //   allDay: true
  // },
  // {
  //   start: addHours(startOfDay(new Date()), 2),
  //   end: new Date(),
  //   title: 'A draggable and resizable event',
  //   color: colors.yellow,
  //   actions: this.actions,
  //   resizable: {
  //     beforeStart: true,
  //     afterEnd: true
  //   },
  //   draggable: true
  // }

  activeDayIsOpen = true;

  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    if (isSameMonth(date, this.viewDate)) {
      if (
        (isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
      }
      this.viewDate = date;
    }
  }

  eventTimesChanged({ event, newStart, newEnd }: CalendarEventTimesChangedEvent): void {
    this.events = this.events.map(iEvent => {
      if (iEvent === event) {
        return {
          ...event,
          start: newStart,
          end: newEnd
        };
      }
      return iEvent;
    });
    this.handleEvent('Dropped or resized', event);
  }

  handleEvent(action: string, event: CalendarEvent): void {
    this.modalData = { event, action };
    this.modal.open(this.modalContent, { size: 'lg' });
  }

  addNewEvent(): void {
    this.newEvents = [
      ...this.newEvents,
      {
        title: 'New event',
        start: startOfDay(new Date()),
        end: endOfDay(new Date()),
        color: colors.red,
        draggable: true,
        resizable: {
          beforeStart: true,
          afterEnd: true
        }
      }
    ];
  }

  ScheduleEvent(newEvent: CalendarEvent) {

    // add to DB
    this.newAppointment.title = newEvent.title;
    this.newAppointment.startDate = new Date(newEvent.start);
    this.newAppointment.endDate = new Date(newEvent.end);
    this.newAppointment.primaryColor = newEvent.color.primary;
    this.newAppointment.secondaryColor = newEvent.color.secondary;
    this.newAppointment.doctorId = this.selectedDoctor;
    this.createAppointment();

    // add all appointments to events again
    this.addAppointmentsToEvents();
    // this.events = [
    //     ... this.events,
    //     {
    //       title: this.addedAppointment.title,
    //       start: new Date(this.addedAppointment.startDate),
    //       end: new Date(this.addedAppointment.endDate),
    //       color: {primary: this.addedAppointment.primaryColor, secondary: this.addedAppointment.secondaryColor},
    //       draggable: true,
    //       resizable: {
    //         beforeStart: true,
    //         afterEnd: true
    //       },
    //       id: this.addedAppointment.id
    //     }
    //   ];

    this.newEvents = this.newEvents.filter(event => event !== newEvent);
    // this.addedAppointment = null;
  }

  addAppointmentsToEvents() {
    this.events = [];

    this.appointments.forEach(a => {
      this.events = [
        ... this.events,
        {
          title: a.title,
          start: new Date(a.startDate),
          end: new Date(a.endDate),
          color: {primary: a.primaryColor, secondary: a.secondaryColor},
          draggable: true,
          resizable: {
            beforeStart: true,
            afterEnd: true
          },
          id: a.id
        }
      ];
    });
  }

  // addEvent(): void {
  //   this.events = [
  //     ...this.events,
  //     {
  //       title: 'New event',
  //       start: startOfDay(new Date()),
  //       end: endOfDay(new Date()),
  //       color: colors.red,
  //       draggable: true,
  //       resizable: {
  //         beforeStart: true,
  //         afterEnd: true
  //       }
  //     }
  //   ];
  // }

  setView(view: CalendarView) {
    this.view = view;
  }

  closeOpenMonthViewDay() {
    this.activeDayIsOpen = false;
  }

  constructor(private modal: NgbModal, private userService: UserService,
              private route: ActivatedRoute,
              private authService: AuthService,
              private alertify: AlertifyService) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
      this.appointments = data['appointments'].result;
      this.doctors = data['doctors'].result;
      this.pagination = data['appointments'].pagination;
    });
    this.selectsParam = 'Selectors';
    // this.loadDoctors();
    this.addAppointmentsToEvents();
    console.log('This is: ' + this.user);

    console.log('This is: ' + this.events.length);


  }

  // Database methods
  loadAppointments() {
    this.userService.getAppointments(this.authService.decodedToken.nameid).subscribe((res: PaginatedResult<Appointment[]>) => {
      this.appointments = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

  loadDoctors() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, null, this.selectsParam)
    .subscribe(
      (res: PaginatedResult<User[]>) => {
      this.doctors = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

    pageChanged(event: any): void {
      this.pagination.currentPage = event.page;
      this.loadAppointments();
    }

    createAppointment() {
      if (this.authService.decodedToken.role === 'Patient') {
        this.newAppointment.patientId = this.user.id;
      }
      if (this.authService.decodedToken.role === 'Doctor') {
        this.newAppointment.doctorId = this.user.id;
      }
      this.userService.createAppointment(this.authService.decodedToken.nameid, this.newAppointment)
        .subscribe((appointment: Appointment) => {
            // this.appointments.unshift(appointment);
            this.appointments.push(appointment);
            // this.newAppointment.title = 'added from ts';
            this.alertify.success('Appointment scheduled');
        }, error => {
          this.alertify.error(error);
        });
    }

    // deleteEvent(eventToDelete: CalendarEvent) {
    //   this.events = this.events.filter(event => event !== eventToDelete);
    // }

    deleteAppointment(id: number) {
      // this.deleteEvent(this.events[id]);

      this.alertify.confirm('Are you sure you want to delete this appointment?', () => {
        this.userService.deleteAppointment(id, this.authService.decodedToken.nameid).subscribe(() => {
          this.appointments.splice(this.appointments.findIndex(m => m.id === id), 1);
          // find index of the message in the messages array that matches the id we're passing in, and we delete it
          this.alertify.success('Appointment has been deleted');
        }, error => {
          this.alertify.error('Failed to delete this appointment');
        });
      });
    }


}
