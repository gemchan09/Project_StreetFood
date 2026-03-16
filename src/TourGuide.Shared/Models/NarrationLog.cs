namespace TouristGuide.Shared.Models;

public class NarrationLog
{
    public int Id { get; set; }
    public int PoiId { get; set; }
    public string? SessionId { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
    public double? DurationSeconds { get; set; }
    public double? UserLatitude { get; set; }
    public double? UserLongitude { get; set; }
}
