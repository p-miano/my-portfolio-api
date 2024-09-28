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
        // Retrieve the current user's Id from token
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Get technology groups associated with the current user
        var groups = _context.UserTechnologyGroups
            .Where(utg => utg.UserId == userId) // Filter by user ID
            .Include(utg => utg.TechnologyGroup) // Include the TechnologyGroup entity
            .ThenInclude(g => g.Technologies) // Include the related Technologies for each group
            .Select(utg => new TechnologyGroupReadDto
            {
                Id = utg.TechnologyGroup.Id,
                Name = utg.TechnologyGroup.Name,
                Technologies = utg.TechnologyGroup.Technologies.Select(t => new TechnologyReadNoGroupDto
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList() // Map the technologies to a simplified DTO
            }).ToList();

        return Ok(groups); // Return the list of technology groups
    }

    // Route: GET /api/technologygroups/{id}
    [HttpGet("{id}")]
    public IActionResult GetTechnologyGroup(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Find the technology group that the current user is associated with
        var group = _context.UserTechnologyGroups
            .Where(utg => utg.UserId == userId && utg.TechnologyGroupId == id) // Filter by user and group ID
            .Include(utg => utg.TechnologyGroup)
            .ThenInclude(g => g.Technologies) // Include related technologies
            .Select(utg => new TechnologyGroupReadDto
            {
                Id = utg.TechnologyGroup.Id,
                Name = utg.TechnologyGroup.Name,
                Technologies = utg.TechnologyGroup.Technologies.Select(t => new TechnologyReadNoGroupDto
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList()
            }).FirstOrDefault();

        if (group == null)
        {
            return NotFound(); // If no group is found for the user, return 404
        }

        return Ok(group); // Return the group if found
    }

    // Route: POST /api/technologygroups
    [HttpPost]
    public IActionResult CreateTechnologyGroup([FromBody] TechnologyGroupCreateDto newGroupDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Return validation errors if the input model is invalid
        }

        // Retrieve user ID from the token
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Format the group name using the helper to ensure consistent capitalization
        var formattedName = StringHelper.FormatTitleCase(newGroupDto.Name);

        // Check if the group already exists with this name globally
        var existingGroup = _context.TechnologyGroups
            .Include(g => g.UserTechnologyGroups) // Include user associations
            .Include(g => g.Technologies)         // Include related technologies
            .FirstOrDefault(g => g.Name == formattedName);

        if (existingGroup != null)
        {
            // Check if the user is already associated with this group
            var userGroupAssociation = existingGroup.UserTechnologyGroups
                .FirstOrDefault(utg => utg.UserId == userId);

            if (userGroupAssociation != null)
            {
                return Conflict("Technology group already exists for this user.");
            }

            // If not, create a new association for the user
            var newUserGroup = new UserTechnologyGroup
            {
                UserId = userId,
                TechnologyGroupId = existingGroup.Id
            };

            _context.UserTechnologyGroups.Add(newUserGroup);
            _context.SaveChanges();

            // Return the existing group with the user association
            return CreatedAtAction(nameof(GetTechnologyGroup), new { id = existingGroup.Id }, new TechnologyGroupReadDto
            {
                Id = existingGroup.Id,
                Name = existingGroup.Name ?? "Unknown",
                Technologies = existingGroup.Technologies?.Select(t => new TechnologyReadNoGroupDto
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList() ?? new List<TechnologyReadNoGroupDto>()
            });
        }

        // If the group doesn't exist, create a new group with the association
        var group = new TechnologyGroup
        {
            Name = formattedName
        };

        _context.TechnologyGroups.Add(group);
        _context.SaveChanges();

        var newUserTechnologyGroup = new UserTechnologyGroup
        {
            UserId = userId,
            TechnologyGroupId = group.Id
        };

        _context.UserTechnologyGroups.Add(newUserTechnologyGroup);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTechnologyGroup), new { id = group.Id }, new TechnologyGroupReadDto
        {
            Id = group.Id,
            Name = group.Name,
            Technologies = new List<TechnologyReadNoGroupDto>() // Empty list for new group
        });
    }

    // Route: PUT /api/technologygroups/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateTechnologyGroup(int id, [FromBody] TechnologyGroupUpdateDto updatedGroupDto)
    {
        // Retrieve the current user's Id from the token
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Retrieve the user from the database
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        // Check if the technology group exists
        var group = _context.TechnologyGroups.Find(id);
        if (group == null)
        {
            return NotFound("Technology Group not found.");
        }

        // Ensure that the user is associated with the technology group
        var userGroup = _context.UserTechnologyGroups.FirstOrDefault(ug => ug.UserId == user.Id && ug.TechnologyGroupId == id);
        if (userGroup == null)
        {
            return Forbid("Bearer");
        }

        // Format the updated group name using StringHelper to ensure title case
        var formattedName = StringHelper.FormatTitleCase(updatedGroupDto.Name);

        // Check if another group with the same name exists globally
        var existingGroup = _context.TechnologyGroups.FirstOrDefault(g => g.Name.ToLower() == formattedName.ToLower() && g.Id != id);
        if (existingGroup != null)
        {
            return Conflict("A technology group with the same name already exists.");
        }

        // Update the group name with the formatted title case
        group.Name = formattedName;

        // Save the changes to the database
        _context.SaveChanges();

        return NoContent();
    }

    // Route: DELETE /api/technologygroups/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteTechnologyGroup(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userTechnologyGroup = _context.UserTechnologyGroups
            .Include(utg => utg.TechnologyGroup)
            .ThenInclude(g => g.Technologies)
            .FirstOrDefault(utg => utg.UserId == userId && utg.TechnologyGroupId == id);

        if (userTechnologyGroup == null)
        {
            return Forbid("Bearer");
        }

        var group = userTechnologyGroup.TechnologyGroup;

        if (group.Technologies.Any())
        {
            return BadRequest("Cannot delete technology group because it has associated technologies.");
        }

        _context.TechnologyGroups.Remove(group);
        _context.SaveChanges();

        return NoContent();
    }
}
