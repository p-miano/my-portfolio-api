using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_portfolio_api.Data;
using my_portfolio_api.Models;

namespace my_portfolio_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TechnologiesController : ControllerBase
{
    private readonly PortfolioContext _context;

    public TechnologiesController(PortfolioContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("api/technologies")]
    public IActionResult GetTechnologies()
    {
        var technologies = _context.Technologies.Select(t => new
        {
            t.Id,
            t.Name
        }).ToList();

        return Ok(technologies);
    }

    [HttpGet("{id}")]
    public IActionResult GetTechnology(int id)
    {
        var technology = _context.Technologies
            .Include(t => t.TechnologyGroup)
            .FirstOrDefault(t => t.Id == id);

        if (technology == null)
        {
            return NotFound();
        }

        return Ok(technology);
    }

    [HttpPost]
    public IActionResult CreateTechnology(Technology technology)
    {
        _context.Technologies.Add(technology);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTechnology), new { id = technology.Id }, technology);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTechnology(int id, Technology updatedTechnology)
    {
        var technology = _context.Technologies.Find(id);
        if (technology == null)
        {
            return NotFound();
        }

        technology.Name = updatedTechnology.Name;
        technology.TechnologyGroupId = updatedTechnology.TechnologyGroupId;
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTechnology(int id)
    {
        var technology = _context.Technologies.Find(id);
        if (technology == null)
        {
            return NotFound();
        }

        _context.Technologies.Remove(technology);
        _context.SaveChanges();

        return NoContent();
    }
}
