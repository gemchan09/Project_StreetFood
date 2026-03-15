using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TouristGuide.Api.Data;
using TouristGuide.Shared.DTOs;
using TouristGuide.Shared.Models;

namespace TouristGuide.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PoisController : ControllerBase
{
    private readonly AppDbContext _db;
    public PoisController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<PoiDto>>> GetAll()
    {
        var pois = await _db.PointsOfInterest.Where(p => p.IsActive).OrderByDescending(p => p.Priority).ToListAsync();
        return pois.Select(MapToDto).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PoiDto>> Get(int id)
    {
        var poi = await _db.PointsOfInterest.FindAsync(id);
        if (poi == null) return NotFound();
        return MapToDto(poi);
    }

    [HttpGet("tour/{tourId}")]
    public async Task<ActionResult<List<PoiDto>>> GetByTour(int tourId)
    {
        var pois = await _db.PointsOfInterest
            .Where(p => p.TourId == tourId && p.IsActive)
            .OrderByDescending(p => p.Priority)
            .ToListAsync();
        return pois.Select(MapToDto).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<PoiDto>> Create(CreatePoiDto dto)
    {
        var poi = new PointOfInterest
        {
            Name = dto.Name, NameEn = dto.NameEn,
            Description = dto.Description, DescriptionEn = dto.DescriptionEn,
            Latitude = dto.Latitude, Longitude = dto.Longitude,
            TriggerRadiusMeters = dto.TriggerRadiusMeters, Priority = dto.Priority,
            NarrationText = dto.NarrationText, NarrationTextEn = dto.NarrationTextEn,
            ImageUrl = dto.ImageUrl, Category = dto.Category, TourId = dto.TourId,
            IsActive = true
        };
        _db.PointsOfInterest.Add(poi);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = poi.Id }, MapToDto(poi));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreatePoiDto dto)
    {
        var poi = await _db.PointsOfInterest.FindAsync(id);
        if (poi == null) return NotFound();

        poi.Name = dto.Name; poi.NameEn = dto.NameEn;
        poi.Description = dto.Description; poi.DescriptionEn = dto.DescriptionEn;
        poi.Latitude = dto.Latitude; poi.Longitude = dto.Longitude;
        poi.TriggerRadiusMeters = dto.TriggerRadiusMeters; poi.Priority = dto.Priority;
        poi.NarrationText = dto.NarrationText; poi.NarrationTextEn = dto.NarrationTextEn;
        poi.ImageUrl = dto.ImageUrl; poi.Category = dto.Category; poi.TourId = dto.TourId;
        poi.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var poi = await _db.PointsOfInterest.FindAsync(id);
        if (poi == null) return NotFound();
        poi.IsActive = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static PoiDto MapToDto(PointOfInterest p) => new()
    {
        Id = p.Id, Name = p.Name, NameEn = p.NameEn,
        Description = p.Description, DescriptionEn = p.DescriptionEn,
        Latitude = p.Latitude, Longitude = p.Longitude,
        TriggerRadiusMeters = p.TriggerRadiusMeters, Priority = p.Priority,
        AudioFileName = p.AudioFileName, AudioFileNameEn = p.AudioFileNameEn,
        NarrationText = p.NarrationText, NarrationTextEn = p.NarrationTextEn,
        ImageUrl = p.ImageUrl, Category = p.Category, IsActive = p.IsActive, TourId = p.TourId
    };
}
