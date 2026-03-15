using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TouristGuide.Api.Data;
using TouristGuide.Shared.DTOs;
using TouristGuide.Shared.Models;

namespace TouristGuide.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AnalyticsController(AppDbContext db) => _db = db;

    [HttpPost("narration")]
    public async Task<IActionResult> LogNarration(NarrationLog log)
    {
        log.PlayedAt = DateTime.UtcNow;
        _db.NarrationLogs.Add(log);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("location")]
    public async Task<IActionResult> LogLocation(LocationLog log)
    {
        log.Timestamp = DateTime.UtcNow;
        _db.LocationLogs.Add(log);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("locations/batch")]
    public async Task<IActionResult> LogLocationsBatch(List<LocationLog> logs)
    {
        foreach (var log in logs)
            log.Timestamp = DateTime.UtcNow;
        _db.LocationLogs.AddRange(logs);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardDto>> GetDashboard()
    {
        var topPois = await _db.NarrationLogs
            .GroupBy(n => n.PoiId)
            .Select(g => new PoiAnalyticsDto
            {
                PoiId = g.Key,
                PlayCount = g.Count(),
                AvgDurationSeconds = g.Average(x => x.DurationSeconds ?? 0)
            })
            .OrderByDescending(x => x.PlayCount)
            .Take(10)
            .ToListAsync();

        // Fill POI names
        var poiIds = topPois.Select(t => t.PoiId).ToList();
        var pois = await _db.PointsOfInterest.Where(p => poiIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id, p => p.Name);
        foreach (var tp in topPois)
            tp.PoiName = pois.GetValueOrDefault(tp.PoiId, "Unknown");

        var heatmap = await _db.LocationLogs
            .GroupBy(l => new { Lat = Math.Round(l.Latitude, 4), Lng = Math.Round(l.Longitude, 4) })
            .Select(g => new HeatmapPointDto
            {
                Latitude = g.Key.Lat,
                Longitude = g.Key.Lng,
                Intensity = g.Count()
            })
            .OrderByDescending(h => h.Intensity)
            .Take(500)
            .ToListAsync();

        return new DashboardDto
        {
            TotalPois = await _db.PointsOfInterest.CountAsync(p => p.IsActive),
            TotalTours = await _db.Tours.CountAsync(t => t.IsActive),
            TotalPlays = await _db.NarrationLogs.CountAsync(),
            TotalSessions = await _db.NarrationLogs.Select(n => n.SessionId).Distinct().CountAsync(),
            TopPois = topPois,
            HeatmapData = heatmap
        };
    }
}
