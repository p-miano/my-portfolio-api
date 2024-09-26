using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_portfolio_api.Data;
using my_portfolio_api.Models;
using my_portfolio_api.DTOs;


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

    [HttpGet]
    [HttpGet]
    public IActionResult GetProjects()
    {
        var projects = _context.Projects
            .Include(p => p.Category) // Include Category for reference
            .Include(p => p.ProjectTechnologies)
            .ThenInclude(pt => pt.Technology) // Include Technologies
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Description,
                p.GithubLink,
                p.DeployedLink,
                p.IsVisible,
                p.StartDate,
                p.EndDate,
                p.Difficulty,
                p.CategoryId,
                p.Category.Name,
                Technologies = p.ProjectTechnologies.Select(pt => new
                {
                    pt.TechnologyId,
                    pt.Technology.Name
                })
            }).ToList();

        return Ok(projects);
    }


    [HttpGet("{id}")]
    public IActionResult GetProject(int id)
    {
        var project = _context.Projects
            .Include(p => p.Category)
            .Include(p => p.ProjectTechnologies)
            .ThenInclude(pt => pt.Technology)
            .FirstOrDefault(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    [HttpPost]
    public IActionResult CreateProject([FromBody] ProjectCreateDto projectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Create the Project entity
        var project = new Project
        {
            Title = projectDto.Title,
            Description = projectDto.Description,
            GithubLink = projectDto.GithubLink,
            DeployedLink = projectDto.DeployedLink,
            IsVisible = projectDto.IsVisible,
            StartDate = projectDto.StartDate,
            EndDate = projectDto.EndDate,
            Difficulty = projectDto.Difficulty,
            CategoryId = projectDto.CategoryId
        };

        // Add the project to the context
        _context.Projects.Add(project);
        _context.SaveChanges();

        // Add the technologies associated with the project
        if (projectDto.TechnologyIds != null && projectDto.TechnologyIds.Count > 0)
        {
            foreach (var techId in projectDto.TechnologyIds)
            {
                _context.ProjectTechnologies.Add(new ProjectTechnology
                {
                    ProjectId = project.Id,
                    TechnologyId = techId
                });
            }
        }

        _context.SaveChanges();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }



    [HttpPut("{id}")]
    public IActionResult UpdateProject(int id, Project updatedProject)
    {
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();
        }

        // Update fields
        project.Title = updatedProject.Title;
        project.Description = updatedProject.Description;
        project.GithubLink = updatedProject.GithubLink;
        project.DeployedLink = updatedProject.DeployedLink;
        project.IsVisible = updatedProject.IsVisible;
        project.StartDate = updatedProject.StartDate;
        project.EndDate = updatedProject.EndDate;
        project.Difficulty = updatedProject.Difficulty;
        project.CategoryId = updatedProject.CategoryId;

        _context.SaveChanges();

        return NoContent();
    }

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
