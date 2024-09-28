using Microsoft.AspNetCore.Authorization;
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
        var technologies = _context.Technologies
            .Select(t => new TechnologyReadDto
            {
                Id = t.Id,
                Name = t.Name,
                TechnologyGroupName = t.TechnologyGroup.Name
            }).ToList();

        return Ok(technologies);
    }

    // Route: GET /api/technologies/{id}
    [HttpGet("{id}")]
    public IActionResult GetTechnology(int id)
    {
        var technology = _context.Technologies
            .Include(t => t.TechnologyGroup)
            .Select(t => new TechnologyReadDto
            {
                Id = t.Id,
                Name = t.Name,
                TechnologyGroupName = t.TechnologyGroup.Name
            })
            .FirstOrDefault(t => t.Id == id);

        if (technology == null)
        {
            return NotFound();
        }

        return Ok(technology);
    }

    [Authorize]
    [HttpPost] // Route: POST /api/technologies
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
            TechnologyGroupId = technologyDto.TechnologyGroupId
        };

        _context.Technologies.Add(technology);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTechnology), new { id = technology.Id }, new TechnologyReadDto
        {
            Id = technology.Id,
            Name = technology.Name,
            TechnologyGroupName = technologyGroup.Name
        });
    }

    [Authorize]
    [HttpPut("{id}")] // Route: PUT /api/technologies/{id}
    public IActionResult UpdateTechnology(int id, [FromBody] TechnologyUpdateDto updatedTechnologyDto)
    {
        var technology = _context.Technologies.Find(id);
        if (technology == null)
        {
            return NotFound("Technology not found.");
        }

        technology.Name = updatedTechnologyDto.Name;
        technology.TechnologyGroupId = updatedTechnologyDto.TechnologyGroupId;
        _context.SaveChanges();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")] // Route: DELETE /api/technologies/{id}
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
