using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name {get; set;}
        public string Description{get; set;}
        public string Location {get; set;}
        public DateTime? PlannedDateTime {get; set;}
        public DateTime CreationDateTime { get; set; }
       public string CreatorUser { get; set; }
        public ICollection<Attendee> Attendees;

    }
} 
