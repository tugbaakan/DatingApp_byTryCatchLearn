using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<EventDto>> GetEvents();
        Task<Event> GetEventByIdAsync(int eventId);
        Task<EventDto> GetEventWithAttendeesByIdAsync(int eventId);
        Task<bool> CheckEvent (int eventId);
        void AddEvent(Event event_var);
        void UpdateEvent(Event event_var);
        void DeleteEvent(Event event_var);
        void GetComments(int eventId);
        void AddComment();
        
    }
} 