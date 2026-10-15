
using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models;

public class Event
{
    [Key]
    public int EventID { get; set; }

    public int OrganiserID { get; set; }

    [Required, MaxLength(150)]
    public string EventName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime EventDate { get; set; }

    [Required, MaxLength(150)]
    public string Location { get; set; } = string.Empty;

    public decimal DistanceKM { get; set; }

    [Required, MaxLength(10)]
    public string EventType { get; set; } = string.Empty;

    public DateTime DateCreated { get; set; } = DateTime.Now;

    public User? Organiser { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    public ICollection<EventRoute> Routes { get; set; } = new List<EventRoute>();
}
