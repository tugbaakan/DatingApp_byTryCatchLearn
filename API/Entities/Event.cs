using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name {get; set;}
        public string Description{get; set;}
        public string Location {get; set;}
        public DateTime? PlannedDateTime {get; set;}
        public DateTime CreationDateTime { get; set; } = DateTime.UtcNow;
        public AppUser CreatorUser { get; set; }
        public int CreatorUserId { get; set; }
        public ICollection<Attendee> Attendees {get; set;}

    }
}