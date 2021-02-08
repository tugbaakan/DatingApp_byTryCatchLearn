import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Event } from 'src/app/_models/event';
import { EventsService } from 'src/app/_services/events.service';

@Component({
  selector: 'app-event-detail',
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.css']
})
export class EventDetailComponent implements OnInit {
  event : Event;

  constructor(private eventService: EventsService, private route: ActivatedRoute ) {
  }

  ngOnInit(): void {
    this.loadEvent();
  }

  loadEvent() {
    this.eventService.getEvent(this.route.snapshot.paramMap.get('id') ).subscribe( event => {
        this.event = event;
      })
    }

}
