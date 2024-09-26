using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_portfolio_api.Data;
using my_portfolio_api.Models;

namespace my_portfolio_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TechnologyGroupsController : ControllerBase
{
    private readonly PortfolioContext _context;

    public TechnologyGroupsController(PortfolioContext context)
    {
        _context = context;
    }

    // Route: GET /api/technologygroups
    [HttpGet]
    public IActionResult GetTechnologyGroups()
    {
        var technologyGroups = _context.TechnologyGroups
            .AsNoTracking()
            .Include(tg => tg.Technologies)
            .ToList() // Load all data into memory before projection
            .Select(tg => new
            {
                tg.Id,
                tg.Name,
                Technologies = tg.Technologies
                    .Select(t => new
                    {
                        t.Id,
                        t.Name
                    })
                    .OrderBy(t => t.Name) // Order technologies alphabetically
                    .ToList()
            })
            .ToList();

        return Ok(technologyGroups);
    }

    // Route: GET /api/technologygroups/{id}
    [HttpGet("{id}")]
    public IActionResult GetTechnologyGroup(int id)
    {
        var group = _context.TechnologyGroups
            .Include(tg => tg.Technologies) // Include Technologies in the specific group
            .Select(tg => new
            {
                tg.Id,
                tg.Name,
                Technologies = tg.Technologies.Select(t => new
                {
                    t.Id,
                    t.Name
                }).ToList()
            })
            .FirstOrDefault(tg => tg.Id == id);

        if (group == null)
        {
            return NotFound();
        }

        return Ok(group);
    }

    // Route: PUT /api/technologygroups/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateTechnologyGroup(int id, [FromBody] TechnologyGroup updatedGroup)
    {
        var group = _context.TechnologyGroups.Find(id);
        if (group == null)
        {
            return NotFound("Technology Group not found.");
        }

        group.Name = updatedGroup.Name;
        _context.SaveChanges();

        return NoContent();
    }

    // Route: DELETE /api/technologygroups/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteTechnologyGroup(int id)
    {
        var group = _context.TechnologyGroups
            .Include(g => g.Technologies) // Include associated technologies
            .FirstOrDefault(g => g.Id == id);

        if (group == null)
        {
            return NotFound("Technology Group not found.");
        }

        if (group.Technologies.Any()) // Check if any technologies are associated
        {
            return BadRequest("Cannot delete technology group because it has associated technologies.");
        }

        _context.TechnologyGroups.Remove(group);
        _context.SaveChanges();

        return NoContent();
    }

}
