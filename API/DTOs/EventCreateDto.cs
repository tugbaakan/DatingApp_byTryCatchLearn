using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs
{
    public class EventCreateDto
    {
        [Required] public string Name {get; set;}
        [Required] public string Description{get; set;}
        [Required] public string Location {get; set;} 
        [Required] public DateTime? PlannedDateTime {get; set;}

    }
} 