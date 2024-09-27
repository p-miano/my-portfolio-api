using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_portfolio_api.Data;
using my_portfolio_api.DTOs;
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
        var groups = _context.TechnologyGroups
            .Include(g => g.Technologies) // Load technologies for each group
            .AsEnumerable() // Perform the projection in-memory
            .Select(g => new TechnologyGroupReadDto
            {
                Id = g.Id,
                Name = g.Name,
                Technologies = g.Technologies.Select(t => new TechnologyReadNoGroupDto
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList()
            }).ToList();

        return Ok(groups);
    }

    // Route: GET /api/technologygroups/{id}
    [HttpGet("{id}")]
    public IActionResult GetTechnologyGroup(int id)
    {
        var group = _context.TechnologyGroups
            .Include(g => g.Technologies) // Load technologies for the group
            .AsEnumerable() // Perform the projection in-memory
            .Select(g => new TechnologyGroupReadDto
            {
                Id = g.Id,
                Name = g.Name,
                Technologies = g.Technologies.Select(t => new TechnologyReadNoGroupDto
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList()
            })
            .FirstOrDefault(g => g.Id == id); // Find the group by ID

        if (group == null)
        {
            return NotFound();
        }

        return Ok(group);
    }

    // Route: POST /api/technologygroups
    [HttpPost]
    public IActionResult CreateTechnologyGroup([FromBody] TechnologyGroupCreateDto newGroupDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var group = new TechnologyGroup
        {
            Name = newGroupDto.Name
        };

        _context.TechnologyGroups.Add(group);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTechnologyGroup), new { id = group.Id }, group);
    }

    // Route: PUT /api/technologygroups/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateTechnologyGroup(int id, [FromBody] TechnologyGroupUpdateDto updatedGroupDto)
    {
        var group = _context.TechnologyGroups.Find(id);
        if (group == null)
        {
            return NotFound("Technology Group not found.");
        }

        group.Name = updatedGroupDto.Name;
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
