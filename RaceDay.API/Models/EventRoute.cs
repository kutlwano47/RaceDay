
using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models;

public class EventRoute
{
    [Key]
    public int RouteID { get; set; }

    public int EventID { get; set; }

    [Required, MaxLength(100)]
    public string RouteName { get; set; } = string.Empty;

    public string? RouteDescription { get; set; }

    public decimal DistanceKM { get; set; }

    public decimal? ElevationGain { get; set; }

    [MaxLength(255)]
    public string? MapURL { get; set; }

    public Event? Event { get; set; }
}
