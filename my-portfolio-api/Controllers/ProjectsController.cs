using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_portfolio_api.Data;
using my_portfolio_api.DTOs;
using my_portfolio_api.Models;

namespace my_portfolio_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly PortfolioContext _context;

    public ProjectsController(PortfolioContext context)
    {
        _context = context;
    }

    // Route: GET /api/projects
    [HttpGet]
    public IActionResult GetProjects()
    {
        var projects = _context.Projects
            .Include(p => p.Category) // Eagerly load Category
            .Include(p => p.ProjectTechnologies)
            .ThenInclude(pt => pt.Technology) // Eagerly load Technology
            .ThenInclude(t => t.TechnologyGroup) // Eagerly load TechnologyGroup
            .Select(p => new ProjectReadDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                GithubLink = p.GithubLink,
                DeployedLink = p.DeployedLink,
                IsVisible = p.IsVisible,
                StartDate = p.StartDate,
                EndDate = p.EndDate,

                // Return both the value and the name of the Difficulty enum
                DifficultyValue = (int)p.Difficulty,
                DifficultyName = p.Difficulty.ToString(),

                CategoryName = p.Category.Name,

                // Map the technologies here
                Technologies = p.ProjectTechnologies.Select(pt => new TechnologyReadDto
                {
                    Id = pt.Technology.Id,
                    Name = pt.Technology.Name,
                    TechnologyGroupName = pt.Technology.TechnologyGroup.Name
                }).ToList()
            }).ToList();

        return Ok(projects);
    }


    // Route: GET /api/projects/{id}
    [HttpGet("{id}")]
    public IActionResult GetProject(int id)
    {
        var project = _context.Projects
            .Include(p => p.Category)
            .Include(p => p.ProjectTechnologies)
            .ThenInclude(pt => pt.Technology)
            .Select(p => new ProjectReadDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                GithubLink = p.GithubLink,
                DeployedLink = p.DeployedLink,
                IsVisible = p.IsVisible,
                StartDate = p.StartDate,
                EndDate = p.EndDate,

                // Return both the value and the name of the Difficulty enum
                DifficultyValue = (int)p.Difficulty,
                DifficultyName = p.Difficulty.ToString(),

                CategoryName = p.Category.Name,
                Technologies = p.ProjectTechnologies.Select(pt => new TechnologyReadDto
                {
                    Id = pt.Technology.Id,
                    Name = pt.Technology.Name,
                    TechnologyGroupName = pt.Technology.TechnologyGroup.Name
                }).ToList()
            })
            .FirstOrDefault(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    // Route: POST /api/projects
    [HttpPost]
    public IActionResult CreateProject([FromBody] ProjectCreateDto projectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Create a new project based on the DTO
        var project = new Project
        {
            Title = projectDto.Title,
            Description = projectDto.Description,
            GithubLink = projectDto.GithubLink,
            DeployedLink = projectDto.DeployedLink,
            IsVisible = projectDto.IsVisible,
            StartDate = projectDto.StartDate,
            EndDate = projectDto.EndDate,
            Difficulty = (DifficultyLevel)projectDto.Difficulty,
            CategoryId = projectDto.CategoryId
        };

        // Add the project to the context
        _context.Projects.Add(project);

        // Associate technologies with the project
        if (projectDto.TechnologyIds != null && projectDto.TechnologyIds.Count > 0)
        {
            foreach (var techId in projectDto.TechnologyIds)
            {
                _context.ProjectTechnologies.Add(new ProjectTechnology
                {
                    Project = project, // Attach the project here, no need to re-fetch it
                    TechnologyId = techId
                });
            }
        }

        // Before saving, attach related entities (Category and Technologies) manually:
        project.Category = _context.Categories.Find(project.CategoryId); // Attach Category
        var technologies = _context.Technologies
            .Where(t => projectDto.TechnologyIds.Contains(t.Id))
            .Include(t => t.TechnologyGroup) // Include TechnologyGroup
            .ToList();

        // Save all the changes
        _context.SaveChanges();

        // Create the response DTO without making an extra database query
        var projectWithDetails = new ProjectReadDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            GithubLink = project.GithubLink,
            DeployedLink = project.DeployedLink,
            IsVisible = project.IsVisible,

            // No formatting, just pass the DateTime? value
            StartDate = project.StartDate,
            EndDate = project.EndDate,

            DifficultyValue = (int)project.Difficulty,
            DifficultyName = project.Difficulty.ToString(),
            CategoryName = project.Category?.Name,
            Technologies = technologies.Select(t => new TechnologyReadDto
            {
                Id = t.Id,
                Name = t.Name,
                TechnologyGroupName = t.TechnologyGroup?.Name
            }).ToList()
        };

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectWithDetails);
    }


    // Route: PUT /api/projects/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateProject(int id, [FromBody] ProjectUpdateDto updatedProjectDto)
    {
        var project = _context.Projects.Include(p => p.ProjectTechnologies).FirstOrDefault(p => p.Id == id);
        if (project == null)
        {
            return NotFound();
        }

        // Update fields
        project.Title = updatedProjectDto.Title;
        project.Description = updatedProjectDto.Description;
        project.GithubLink = updatedProjectDto.GithubLink;
        project.DeployedLink = updatedProjectDto.DeployedLink;
        project.IsVisible = updatedProjectDto.IsVisible;
        project.StartDate = updatedProjectDto.StartDate;
        project.EndDate = updatedProjectDto.EndDate;

        // Convert the integer Difficulty to the Enum
        project.Difficulty = (DifficultyLevel)updatedProjectDto.Difficulty;

        project.CategoryId = updatedProjectDto.CategoryId;

        // Handle updating technologies if provided
        if (updatedProjectDto.TechnologyIds != null && updatedProjectDto.TechnologyIds.Count > 0)
        {
            // Remove old associations
            _context.ProjectTechnologies.RemoveRange(project.ProjectTechnologies);
            // Add new associations
            foreach (var techId in updatedProjectDto.TechnologyIds)
            {
                _context.ProjectTechnologies.Add(new ProjectTechnology { ProjectId = id, TechnologyId = techId });
            }
        }

        _context.SaveChanges();

        return NoContent();
    }

    // Route: DELETE /api/projects/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteProject(int id)
    {
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();
        }

        _context.Projects.Remove(project);
        _context.SaveChanges();

        return NoContent();
    }
}
