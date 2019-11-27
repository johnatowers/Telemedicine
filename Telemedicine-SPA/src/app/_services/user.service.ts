import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { Message } from '../_models/message';
import { Appointment } from '../_models/Appointment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUsers(page?, itemsPerPage?, userParams?, selectsParam?): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (selectsParam === 'Selectors') {
      params = params.append('selectors', 'true');
    }

    if (selectsParam === 'Selectees') {
      params = params.append('selectees', 'true');
    }

    if (selectsParam === 'All') {
       params = params.append('doctorRoleOnly', 'true');
    }

    return this.http.get<User[]>(this.baseUrl + 'users', { observe: 'response', params}).pipe(map(response => {
      paginatedResult.result = response.body;
      if (response.headers.get('Pagination') != null) {
        paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
      }
      return paginatedResult;
    }));
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }

  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user);
  }

  deleteDocument(userId: number, id: number) {
    return this.http.delete(this.baseUrl + 'users/' + userId + '/documents/' + id);
  }

  sendSelect(id: number, recipientId: number) {
    return this.http.post(this.baseUrl + 'users/' + id + '/select/' + recipientId, {});
  }

  // setMainPhoto(userId: number, id: number){
  //   return this.http.post(this.baseUrl + 'users/' + userId + '/photos' + id + '/setMain', {} );
  // }

  getMessages(id: number, page?, itemsPerPage?, messageContainer?) {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();
    let params = new HttpParams();
    params = params.append('MessageContainer', messageContainer);
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.http.get<Message[]>(this.baseUrl + 'users/' + id + '/message', {observe: 'response', params})
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  createAppointment(id: number, appointment: Appointment) {
    return this.http.post(this.baseUrl + 'users/' + id + '/appointment/', appointment);
  }

  // get appointment paginated version
  getAppointments(id: number, page?, itemsPerPage?) {
    const paginatedResult: PaginatedResult<Appointment[]> = new PaginatedResult<Appointment[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.http.get<Appointment[]>(this.baseUrl + 'users/' + id + '/appointment', {observe: 'response', params})
    .pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  deleteAppointment(id: number, userId: number) {
    return this.http.post(this.baseUrl + 'users/' + userId + '/appointment/' + id, {});
  }

  getMessageThread(id: number, recipientId: number) {
    return this.http.get<Message[]>(this.baseUrl + 'users/' + id + '/message/thread/' + recipientId);
  }

  sendMessage(id: number, message: Message) {
    return this.http.post(this.baseUrl + 'users/' + id + '/message', message);
  }

  deleteMessage(id: number, userId: number) {
    return this.http.post(this.baseUrl + 'users/' + userId + '/message/' + id, {});
  }

  markAsRead(userId: number, messageId: number) {
    this.http.post(this.baseUrl + 'users/' + userId + '/message/' + messageId + '/read', {}).subscribe();
  }

}
