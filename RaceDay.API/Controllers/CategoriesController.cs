using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly RaceDayDbContext _context;
    public CategoriesController(RaceDayDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Categories.ToListAsync());

    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetByEvent(int eventId) => Ok(await _context.Categories.Where(c => c.EventID == eventId).ToListAsync());

    [Authorize(Roles = "Admin,Organiser")]
    [HttpPost]
    public async Task<IActionResult> Create(Category model)
    {
        _context.Categories.Add(model);
        await _context.SaveChangesAsync();
        return Ok(model);
    }

    [Authorize(Roles = "Admin,Organiser")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cat = await _context.Categories.FindAsync(id);
        if (cat == null) return NotFound();
        _context.Categories.Remove(cat);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}