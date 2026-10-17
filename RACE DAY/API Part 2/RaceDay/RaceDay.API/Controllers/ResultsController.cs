using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResultsController : ControllerBase
{
    private readonly RaceDayDbContext _context;
    public ResultsController(RaceDayDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Results.ToListAsync());

    [HttpGet("enrolment/{enrolmentId}")]
    public async Task<IActionResult> GetByEnrolment(int enrolmentId)
    {
        var result = await _context.Results.FirstOrDefaultAsync(r => r.EnrolmentID == enrolmentId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Organiser")]
    [HttpPost]
    public async Task<IActionResult> Create(Result model)
    {
        model.DateRecorded = DateTime.Now;
        _context.Results.Add(model);
        await _context.SaveChangesAsync();
        return Ok(model);
    }
}