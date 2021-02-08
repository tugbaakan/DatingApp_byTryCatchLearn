import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Event } from '../_models/event';

@Injectable({
  providedIn: 'root'
})
export class EventsService {
  baseUrl = environment.apiUrl;
  events: Event[] = [];

  constructor(private http: HttpClient) {
  }

  getEvents( ) {
    return this.http.get<Event[]>(this.baseUrl + 'events');    
  }

  getEvent(id: string) {
    return this.http.get<Event>(this.baseUrl + 'events/attendees/' + id);    
  }

  attendEvent(id: number) {
    return this.http.put(this.baseUrl + 'events/attend/' + id, {});
  }

}