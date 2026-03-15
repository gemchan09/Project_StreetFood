using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TouristGuide.Api.Data;
using TouristGuide.Shared.Models;

namespace TouristGuide.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToursController : ControllerBase
{
    private readonly AppDbContext _db;
    public ToursController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<Tour>>> GetAll()
    {
        return await _db.Tours.Where(t => t.IsActive).Include(t => t.Pois.Where(p => p.IsActive)).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Tour>> Get(int id)
    {
        var tour = await _db.Tours.Include(t => t.Pois.Where(p => p.IsActive)).FirstOrDefaultAsync(t => t.Id == id);
        if (tour == null) return NotFound();
        return tour;
    }

    [HttpPost]
    public async Task<ActionResult<Tour>> Create(Tour tour)
    {
        tour.CreatedAt = DateTime.UtcNow;
        _db.Tours.Add(tour);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = tour.Id }, tour);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Tour tour)
    {
        var existing = await _db.Tours.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Name = tour.Name;
        existing.NameEn = tour.NameEn;
        existing.Description = tour.Description;
        existing.DescriptionEn = tour.DescriptionEn;
        existing.ImageUrl = tour.ImageUrl;
        existing.IsActive = tour.IsActive;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tour = await _db.Tours.FindAsync(id);
        if (tour == null) return NotFound();
        tour.IsActive = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
