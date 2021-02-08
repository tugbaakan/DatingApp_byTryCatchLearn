using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class EventRepository : IEventRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public EventRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<IEnumerable<EventDto>> GetEvents()
        {
            return await _context.Events
                    .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
        }
        public async Task<bool> CheckEvent(int eventId)
        {
            return await _context.Events
                    .AnyAsync(x => x.Id == eventId);
        }
        public async Task<Event> GetEventByIdAsync(int eventId)
        {
            return await _context.Events.FindAsync(eventId);
        }
        public async Task<EventDto> GetEventWithAttendeesByIdAsync(int eventId)
        {
            return await _context.Events
                    .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(x => x.Id == eventId);
        }

        public void AddEvent(Event event_var)
        {
            _context.Events.Add(event_var);
        }
        public void UpdateEvent(Event event_var)
        {
            _context.Events.Update(event_var);
        }
        public void DeleteEvent(Event event_var)
        {
            _context.Events.Remove(event_var);
        }
        void IEventRepository.AddComment()
        {
            throw new System.NotImplementedException();
        }

        void IEventRepository.GetComments(int eventId)
        {
            throw new System.NotImplementedException();
        }


    }
}