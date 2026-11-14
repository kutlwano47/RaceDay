using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventRoutesController : ControllerBase
{
    private readonly RaceDayDbContext _context;
    public EventRoutesController(RaceDayDbContext context) => _context = context;

    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetByEvent(int eventId) => Ok(await _context.EventRoutes.Where(r => r.EventID == eventId).ToListAsync());

    [Authorize(Roles = "Admin,Organiser")]
    [HttpPost]
    public async Task<IActionResult> Create(EventRoute model)
    {
        _context.EventRoutes.Add(model);
        await _context.SaveChangesAsync();
        return Ok(model);
    }

    [Authorize(Roles = "Admin,Organiser")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var route = await _context.EventRoutes.FindAsync(id);
        if (route == null) return NotFound();
        _context.EventRoutes.Remove(route);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}