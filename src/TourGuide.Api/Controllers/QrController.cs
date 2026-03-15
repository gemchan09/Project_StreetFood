using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TouristGuide.Api.Data;

namespace TouristGuide.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QrController : ControllerBase
{
    private readonly AppDbContext _db;
    public QrController(AppDbContext db) => _db = db;

    /// <summary>
    /// Generate QR code SVG for a POI
    /// </summary>
    [HttpGet("poi/{poiId}")]
    public async Task<IActionResult> GetQrForPoi(int poiId)
    {
        var poi = await _db.PointsOfInterest.FindAsync(poiId);
        if (poi == null) return NotFound();

        var qrData = $"poi:{poiId}";
        var svg = GenerateQrSvg(qrData, poi.Name);
        return Content(svg, "image/svg+xml");
    }

    /// <summary>
    /// Get all POIs with their QR data
    /// </summary>
    [HttpGet("all")]
    public async Task<ActionResult<List<QrInfoDto>>> GetAllQr()
    {
        var pois = await _db.PointsOfInterest
            .Where(p => p.IsActive)
            .Select(p => new QrInfoDto
            {
                PoiId = p.Id,
                PoiName = p.Name,
                QrData = $"poi:{p.Id}",
                Category = p.Category
            })
            .ToListAsync();
        return pois;
    }

    private static string GenerateQrSvg(string data, string label)
    {
        // Simple QR-like SVG placeholder (for actual QR, the CMS uses JS library)
        // This returns a styled card with the QR data for printing
        return $@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""300"" height=""350"" viewBox=""0 0 300 350"">
  <rect width=""300"" height=""350"" fill=""white"" stroke=""#333"" stroke-width=""2"" rx=""12""/>
  <rect x=""50"" y=""30"" width=""200"" height=""200"" fill=""#f0f0f0"" stroke=""#999"" stroke-width=""1"" rx=""8""/>
  <text x=""150"" y=""140"" text-anchor=""middle"" font-size=""24"" font-family=""monospace"" fill=""#333"">{data}</text>
  <text x=""150"" y=""170"" text-anchor=""middle"" font-size=""12"" fill=""#666"">Quét để nghe thuyết minh</text>
  <text x=""150"" y=""270"" text-anchor=""middle"" font-size=""14"" font-weight=""bold"" fill=""#333"">{System.Security.SecurityElement.Escape(label)}</text>
  <text x=""150"" y=""295"" text-anchor=""middle"" font-size=""11"" fill=""#888"">Tourist Audio Guide</text>
</svg>";
    }
}

public class QrInfoDto
{
    public int PoiId { get; set; }
    public string PoiName { get; set; } = string.Empty;
    public string QrData { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
