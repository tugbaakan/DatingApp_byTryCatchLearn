namespace API.Entities
{
    public class Attendee
    {
        public int Id { get; set; }
        public Event Event {get; set;}
        public int EventId {get; set;}
        public AppUser User {get; set;}
        public int UserId {get; set;}
    }
}