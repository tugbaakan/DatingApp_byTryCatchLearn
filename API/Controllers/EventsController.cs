using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EventsController: BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EventsController(IMapper mapper, IUnitOfWork unitOfWork)
        {  
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            var events = await _unitOfWork.EventRepository.GetEvents();
            return Ok(events);

        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEventById(int id)
        {
            if( ! await _unitOfWork.EventRepository.CheckEvent(id))
                return BadRequest("Wrong event id!");
            var event_var = await _unitOfWork.EventRepository.GetEventByIdAsync(id);
            return Ok(event_var);

        }
        
        [HttpGet("attendees/{id}")]
        public async Task<ActionResult<IEnumerable<AttendeeDto>>> GetAttendeesByEventId(int id)
        {
            if( ! await _unitOfWork.EventRepository.CheckEvent(id))
                return BadRequest("Wrong event id!");
            var event_var = await _unitOfWork.AttendeeRepository.GetAttendees(id);
            return Ok(event_var);

        }
    
        [HttpPost("create")]
        public async Task<ActionResult> CreateEvent ( EventCreateDto eventDto)
        {
            int userId = User.GetUserId();
            
            var event_var = new Event{
                Name = eventDto.Name,
                Description = eventDto.Description,
                Location = eventDto.Location,
                PlannedDateTime = eventDto.PlannedDateTime,
                CreatorUserId = userId
            };
            
            _unitOfWork.EventRepository.AddEvent(event_var);

            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Failed to create event");
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateEvent (EventUpdateDto eventDto)
        {
            int userId = User.GetUserId();
            
            if( ! await _unitOfWork.EventRepository.CheckEvent(eventDto.Id))
                return BadRequest("Wrong event id!");

            var eventTobeUpdated = await _unitOfWork.EventRepository.GetEventByIdAsync(eventDto.Id);
            
            if ( eventTobeUpdated.CreatorUserId != userId)
                return BadRequest("You cannot update an event that you did not create!");

            eventTobeUpdated.Name = eventDto.Name;
            eventTobeUpdated.Description = eventDto.Description;
            eventTobeUpdated.Location = eventDto.Location;
            eventTobeUpdated.PlannedDateTime = eventDto.PlannedDateTime;
            
            _unitOfWork.EventRepository.UpdateEvent(eventTobeUpdated);
            
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Failed to update event");
        }

        [HttpPost("cancel/{eventId}")]
        public async Task<ActionResult> CancelEvent (int eventId)
        {
            int userId = User.GetUserId();
            
            if( ! await _unitOfWork.EventRepository.CheckEvent(eventId))
                return BadRequest("Wrong event id!");

            var eventToBeDeleted = await _unitOfWork.EventRepository.GetEventByIdAsync(eventId);
            
            if ( eventToBeDeleted.CreatorUserId != userId)
                return BadRequest("You cannot cancel an event that you did not create!");

            _unitOfWork.EventRepository.DeleteEvent(eventToBeDeleted);
            
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Failed to cancel event");
        }

        [HttpPost("attend/{eventId}")]
        public async Task<ActionResult> AttendEvent(int eventId)
        {
            int userId = User.GetUserId();

            if( ! await _unitOfWork.EventRepository.CheckEvent(eventId))
                return BadRequest("Wrong event id!");

            if (  await _unitOfWork.AttendeeRepository.CheckAttendee( eventId, userId))
                return BadRequest("You already attended that event!");
                        
            var attendee = new Attendee{
                EventId = eventId,
                UserId = userId
            };

            _unitOfWork.AttendeeRepository.AddAttendee(attendee);
           
            if( await _unitOfWork.Complete()) return Ok();
            return BadRequest("Failed to attend event");

        }

        [HttpPost("unattend/{eventId}")]
        public async Task<ActionResult> UnAttendEvent(int eventId)
        {
            int userId = User.GetUserId();

            if( ! await _unitOfWork.EventRepository.CheckEvent(eventId))
                return BadRequest("Wrong event id!");

            if (! await _unitOfWork.AttendeeRepository.CheckAttendee( eventId, userId))
                return BadRequest("You cannot unattend an event that you did not attend!");

            var attendee = await _unitOfWork.AttendeeRepository.GetAttendee(eventId, userId);

            _unitOfWork.AttendeeRepository.RemoveAttendee(attendee);

            if( await _unitOfWork.Complete()) return Ok();
            return BadRequest("Failed to unattend event");

        }


    }
}