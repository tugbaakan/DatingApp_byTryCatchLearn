import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Event } from 'src/app/_models/event';
import { EventsService } from 'src/app/_services/events.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {
  events: Event[];

  constructor(private eventService: EventsService, private toastr: ToastrService ) {
  }

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents() {
    this.eventService.getEvents().subscribe( events => {
        this.events = events;
      })
    }

  attendEvent(id: number) {
    this.eventService.attendEvent(id).subscribe(() => {
      this.toastr.success('Attended successfully');
    })


  }
  
}