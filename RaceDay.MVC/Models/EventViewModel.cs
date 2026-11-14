using System.Text.Json.Serialization;

namespace RaceDay.MVC.Models
{
    public class EventViewModel
    {
        [JsonPropertyName("eventID")]
        public int Id { get; set; }

        [JsonPropertyName("eventName")]
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        [JsonPropertyName("eventDate")]
        public DateTime Date { get; set; } = DateTime.Now;

        public string Location { get; set; } = "";

        [JsonPropertyName("distanceKM")]
        public double Distance { get; set; }

        public string EventType { get; set; } = "";
        public string? BannerUrl { get; set; }
        public int OrganiserId { get; set; }
    }
}