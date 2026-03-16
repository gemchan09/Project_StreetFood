namespace TouristGuide.Shared.Models;

public class PointOfInterest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double TriggerRadiusMeters { get; set; } = 50;
    public int Priority { get; set; } = 1;
    public string? AudioFileName { get; set; }
    public string? AudioFileNameEn { get; set; }
    public string? NarrationText { get; set; }
    public string? NarrationTextEn { get; set; }
    public string? ImageUrl { get; set; }
    public string Category { get; set; } = "landmark";
    public bool IsActive { get; set; } = true;
    public int? TourId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
