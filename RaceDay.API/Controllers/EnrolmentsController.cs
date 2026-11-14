using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrolmentsController : ControllerBase
{
    private readonly RaceDayDbContext _context;
    public EnrolmentsController(RaceDayDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Enrolments.Include(e => e.Category).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var en = await _context.Enrolments.FindAsync(id);
        if (en == null) return NotFound();
        return Ok(en);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Enrolment model)
    {
        model.EnrolmentDate = DateTime.Now;
        _context.Enrolments.Add(model);
        await _context.SaveChangesAsync();
        return Ok(model);
    }
}