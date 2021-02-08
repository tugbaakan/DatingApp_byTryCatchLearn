using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AttendeeRepository : IAttendeeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public AttendeeRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<AttendeeDto>> GetAttendees(int eventId)
        {
            return await _context.Attendees
                    .Where(x => x.EventId == eventId )
                    .ProjectTo<AttendeeDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
        }
        public async Task<Attendee> GetAttendee(int eventId, int userId)
        {
            return await _context.Attendees.SingleOrDefaultAsync(x => x.EventId == eventId 
                && x.UserId == userId );
        }
        public async Task<bool> CheckAttendee(int eventId, int userId)
        {
            return await _context.Attendees
                    .AnyAsync(x => x.EventId == eventId && x.UserId == userId);
        }
        public void AddAttendee(Attendee attendee)
        {
            _context.Attendees.Add(attendee);
        }
        public void RemoveAttendee(Attendee attendee)
        {
            _context.Attendees.Remove(attendee);
        }
    }
}