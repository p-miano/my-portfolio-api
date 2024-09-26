using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_portfolio_api.Data;
using my_portfolio_api.DTOs;
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

    // Route: GET /api/technologies
    [HttpGet]
    public IActionResult GetTechnologies()
    {
        var technologies = _context.Technologies.Select(t => new
        {
            t.Id,
            t.Name
        }).ToList();

        return Ok(technologies);
    }

    // Route: GET /api/technologies/{id}
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

    // Route: POST /api/technologies
    [HttpPost]
    public IActionResult CreateTechnology([FromBody] TechnologyCreateDto technologyDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check if TechnologyGroup exists
        var technologyGroup = _context.TechnologyGroups.Find(technologyDto.TechnologyGroupId);
        if (technologyGroup == null)
        {
            return NotFound($"Technology Group with ID {technologyDto.TechnologyGroupId} not found.");
        }

        // Create the technology based on the DTO
        var technology = new Technology
        {
            Name = technologyDto.Name,
            TechnologyGroupId = technologyDto.TechnologyGroupId,
            TechnologyGroup = technologyGroup
        };

        _context.Technologies.Add(technology);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTechnology), new { id = technology.Id }, technology);
    }


    // Route: PUT /api/technologies/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateTechnology(int id, [FromBody] Technology updatedTechnology)
    {
        var technology = _context.Technologies.Find(id);
        if (technology == null)
        {
            return NotFound("Technology not found.");
        }

        technology.Name = updatedTechnology.Name;
        technology.TechnologyGroupId = updatedTechnology.TechnologyGroupId;
        _context.SaveChanges();

        return NoContent();
    }

    // Route: DELETE /api/technologies/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteTechnology(int id)
    {
        var technology = _context.Technologies
            .Include(t => t.ProjectTechnologies) // Include associated projects
            .FirstOrDefault(t => t.Id == id);

        if (technology == null)
        {
            return NotFound("Technology not found.");
        }

        if (technology.ProjectTechnologies.Any()) // Check if any projects are associated
        {
            return BadRequest("Cannot delete technology because it is associated with projects.");
        }

        _context.Technologies.Remove(technology);
        _context.SaveChanges();

        return NoContent();
    }

}
