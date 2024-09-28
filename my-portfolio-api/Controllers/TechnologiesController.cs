using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_portfolio_api.Data;
using my_portfolio_api.DTOs;
using my_portfolio_api.Models;
using my_portfolio_api.Utils;
using System.Security.Claims;

namespace my_portfolio_api.Controllers;

[Authorize]
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
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Get technologies explicitly associated with the current user (through UserTechnology)
        var technologies = _context.UserTechnologies
            .Where(ut => ut.UserId == userId) // Filter by current user's ID
            .Include(ut => ut.Technology.TechnologyGroup) // Include the technology group for display purposes
            .Select(ut => new TechnologyReadDto
            {
                Id = ut.Technology.Id,
                Name = ut.Technology.Name,
                TechnologyGroupName = ut.Technology.TechnologyGroup.Name
            }).ToList(); // Project the result into a simplified DTO

        return Ok(technologies); // Return the list of technologies
    }

    // Route: GET /api/technologies/{id}
    [HttpGet("{id}")]
    public IActionResult GetTechnology(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Retrieve the technology only if it is explicitly associated with the user
        var technology = _context.UserTechnologies
            .Where(ut => ut.UserId == userId && ut.TechnologyId == id) // Filter by user ID and technology ID
            .Include(ut => ut.Technology.TechnologyGroup) // Include the technology group for display
            .Select(ut => new TechnologyReadDto
            {
                Id = ut.Technology.Id,
                Name = ut.Technology.Name,
                TechnologyGroupName = ut.Technology.TechnologyGroup.Name
            }).FirstOrDefault();

        if (technology == null)
        {
            return NotFound(); // Return 404 if no technology is found for the user
        }

        return Ok(technology); // Return the found technology
    }

    // Route: POST /api/technologies
    [HttpPost]
    public IActionResult CreateTechnology([FromBody] TechnologyCreateDto technologyDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Return validation errors if the input model is invalid
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Check if the TechnologyGroup exists and if the user is associated with it
        var technologyGroup = _context.TechnologyGroups
            .Include(tg => tg.UserTechnologyGroups) // Include UserTechnologyGroups to check associations
            .FirstOrDefault(tg => tg.Id == technologyDto.TechnologyGroupId &&
                                   tg.UserTechnologyGroups.Any(utg => utg.UserId == userId)); // Filter by user and group ID

        if (technologyGroup == null)
        {
            return NotFound($"Technology Group with ID {technologyDto.TechnologyGroupId} not found or not associated with the user.");
        }

        var formattedName = StringHelper.FormatTitleCase(technologyDto.Name); // Ensure the technology name is in title case

        // Check if the technology already exists globally within this group
        var existingTechnology = _context.Technologies
            .Include(t => t.UserTechnologies)
            .FirstOrDefault(t => t.Name.ToLower() == formattedName.ToLower() && t.TechnologyGroupId == technologyDto.TechnologyGroupId);

        if (existingTechnology != null)
        {
            // Check if the user is already associated with the existing technology
            var userTechAssociation = existingTechnology.UserTechnologies
                .FirstOrDefault(ut => ut.UserId == userId);

            if (userTechAssociation != null)
            {
                return Conflict("Technology already exists for this user in the specified group.");
            }

            // If not, create a new association for the user
            var newUserTechnology = new UserTechnology
            {
                UserId = userId,
                TechnologyId = existingTechnology.Id
            };

            _context.UserTechnologies.Add(newUserTechnology);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTechnology), new { id = existingTechnology.Id }, new TechnologyReadDto
            {
                Id = existingTechnology.Id,
                Name = existingTechnology.Name,
                TechnologyGroupName = technologyGroup.Name
            });
        }

        // If the technology doesn't exist, create a new technology and associate it with the user
        var newTechnology = new Technology
        {
            Name = formattedName,
            TechnologyGroupId = technologyDto.TechnologyGroupId,
            UserTechnologies = new List<UserTechnology>
            {
                new UserTechnology { UserId = userId } // Associate the new technology with the current user
            }
        };

        _context.Technologies.Add(newTechnology);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTechnology), new { id = newTechnology.Id }, new TechnologyReadDto
        {
            Id = newTechnology.Id,
            Name = newTechnology.Name,
            TechnologyGroupName = technologyGroup.Name
        });
    }

    // Route: PUT /api/technologies/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateTechnology(int id, [FromBody] TechnologyUpdateDto updatedTechnologyDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Retrieve the technology if it belongs to the user (through UserTechnology association)
        var technology = _context.Technologies
            .Include(t => t.TechnologyGroup)
            .Include(t => t.UserTechnologies) // Include UserTechnologies to check user association
            .FirstOrDefault(t => t.Id == id && t.UserTechnologies.Any(ut => ut.UserId == userId)); // Check user association

        if (technology == null)
        {
            // Return 403 if the technology is not associated with the current user
            return Forbid("Bearer");
        }

        var formattedName = StringHelper.FormatTitleCase(updatedTechnologyDto.Name); // Format the technology name

        // Check if another technology with the same name exists in this group
        var existingTechnology = _context.Technologies
            .FirstOrDefault(t => t.Name.ToLower() == formattedName.ToLower() &&
                                 t.TechnologyGroupId == updatedTechnologyDto.TechnologyGroupId &&
                                 t.Id != id);

        if (existingTechnology != null)
        {
            return Conflict("A technology with the same name already exists in this group.");
        }

        // Check if the user is associated with the target TechnologyGroup
        var targetTechnologyGroup = _context.TechnologyGroups
            .Include(tg => tg.UserTechnologyGroups)
            .FirstOrDefault(tg => tg.Id == updatedTechnologyDto.TechnologyGroupId &&
                                  tg.UserTechnologyGroups.Any(utg => utg.UserId == userId)); // Check if the user is associated with the group

        if (targetTechnologyGroup == null)
        {
            // Return 403 if the user is not associated with the target TechnologyGroup
            return Forbid("Bearer");
        }

        // Update the technology
        technology.Name = formattedName;
        technology.TechnologyGroupId = updatedTechnologyDto.TechnologyGroupId;

        _context.SaveChanges();

        return NoContent(); // Return 204 No Content after a successful update
    }

    // Route: DELETE /api/technologies/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteTechnology(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var technology = _context.Technologies
            .Include(t => t.TechnologyGroup.UserTechnologyGroups)
            .FirstOrDefault(t => t.Id == id && t.TechnologyGroup.UserTechnologyGroups.Any(utg => utg.UserId == userId)); // Check user association

        if (technology == null)
        {
            return Forbid("Bearer"); // Return 403 if the technology is not associated with the user
        }

        if (technology.ProjectTechnologies.Any()) // Check if the technology is associated with projects
        {
            return BadRequest("Cannot delete technology because it is associated with projects.");
        }

        _context.Technologies.Remove(technology); // Remove the technology
        _context.SaveChanges();

        return NoContent(); // Return 204 No Content after successful deletion
    }
}

