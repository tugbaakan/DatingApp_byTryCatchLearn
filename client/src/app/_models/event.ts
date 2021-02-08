import { Attendee } from "./attendee";

export interface Event {
    id: number;
    name: string;
    description: string;
    location: string;
    plannedDateTime: Date;
    creatorUser: string;
    attendees: Attendee[];
  }