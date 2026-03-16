namespace TouristGuide.Shared.Models;

public class LocationLog
{
    public int Id { get; set; }
    public string? SessionId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
