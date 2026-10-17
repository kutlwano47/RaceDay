using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly RaceDayDbContext _context;
    public EventsController(RaceDayDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Events.Include(e => e.Categories).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ev = await _context.Events.Include(e => e.Categories).FirstOrDefaultAsync(e => e.EventID == id);
        if (ev == null) return NotFound();
        return Ok(ev);
    }

    [Authorize(Roles = "Admin,Organiser")]
    [HttpPost]
    public async Task<IActionResult> Create(Event model)
    {
        model.DateCreated = DateTime.Now;
        _context.Events.Add(model);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.EventID }, model);
    }

    [Authorize(Roles = "Admin,Organiser")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Event model)
    {
        if (id != model.EventID) return BadRequest("ID mismatch");
        var existing = await _context.Events.FindAsync(id);
        if (existing == null) return NotFound();
        existing.EventName = model.EventName;
        existing.Description = model.Description;
        existing.EventDate = model.EventDate;
        existing.Location = model.Location;
        existing.DistanceKM = model.DistanceKM;
        existing.EventType = model.EventType;
        existing.OrganiserID = model.OrganiserID;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ev = await _context.Events.FindAsync(id);
        if (ev == null) return NotFound();
        _context.Events.Remove(ev);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}