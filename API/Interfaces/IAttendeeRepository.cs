using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IAttendeeRepository
    {
        Task<IEnumerable<AttendeeDto>> GetAttendees(int eventId);
        Task<Attendee> GetAttendee(int eventId, int userId);
        Task<bool> CheckAttendee (int eventId, int userId);
        void AddAttendee(Attendee attendee);
        void RemoveAttendee(Attendee attendee);
    }
}