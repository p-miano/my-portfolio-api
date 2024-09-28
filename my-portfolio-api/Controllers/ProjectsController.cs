using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_portfolio_api.Data;
using my_portfolio_api.DTOs;
using my_portfolio_api.Models;
using my_portfolio_api.Utils;
using System.Security.Claims;

namespace my_portfolio_api.Controllers
{
    [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly PortfolioContext _context;

        public ProjectsController(PortfolioContext context)
        {
            _context = context;
        }

        // GET /api/projects - Retrieves all projects belonging to the authenticated user
        [HttpGet]
        public IActionResult GetProjects()
        {
            // Get the current user's ID from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Fetch the projects associated with the current user
            var projects = _context.Projects
                .Where(p => p.UserId == userId) // Filter projects by user ID
                .Include(p => p.Category) // Include the related category for each project
                .Include(p => p.ProjectTechnologies)
                .ThenInclude(pt => pt.Technology) // Include related technologies
                .ThenInclude(t => t.TechnologyGroup) // Include technology groups
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
                    DifficultyValue = (int)p.Difficulty,
                    DifficultyName = p.Difficulty.ToString(),
                    CategoryName = p.Category.Name,
                    Technologies = p.ProjectTechnologies.Select(pt => new TechnologyReadDto
                    {
                        Id = pt.Technology.Id,
                        Name = pt.Technology.Name,
                        TechnologyGroupName = pt.Technology.TechnologyGroup.Name
                    }).ToList() // Convert each technology to a DTO
                }).ToList();

            // Return the user's projects as a response
            return Ok(projects);
        }

        // GET /api/projects/{id} - Retrieves a specific project by its ID
        [HttpGet("{id}")]
        public IActionResult GetProject(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Fetch the project if it belongs to the current user
            var project = _context.Projects
                .Where(p => p.UserId == userId && p.Id == id) // Ensure the user owns the project
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
                .FirstOrDefault();

            // If the project doesn't exist or doesn't belong to the user, return 404
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // POST /api/projects - Creates a new project for the authenticated user
        [HttpPost]
        public IActionResult CreateProject([FromBody] ProjectCreateDto projectDto)
        {
            // Validate the model and request data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure the difficulty level is valid based on the Enum values
            if (!Enum.IsDefined(typeof(DifficultyLevel), projectDto.Difficulty))
            {
                return BadRequest("Invalid difficulty level.");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Ensure that the user owns the category they are trying to use
            var category = _context.UserCategories
                .Include(uc => uc.Category)
                .FirstOrDefault(uc => uc.CategoryId == projectDto.CategoryId && uc.UserId == userId)?.Category;

            if (category == null)
            {
                return BadRequest("The user is not associated with the specified category.");
            }

            // Ensure all selected technologies belong to the user
            var technologies = _context.Technologies
                .Include(t => t.UserTechnologies)
                .Where(t => projectDto.TechnologyIds.Contains(t.Id) && t.UserTechnologies.Any(ut => ut.UserId == userId))
                .Include(t => t.TechnologyGroup) // Include technology groups for the technologies
                .ToList();

            // If any technology IDs do not belong to the user, return an error
            if (technologies.Count != projectDto.TechnologyIds.Count)
            {
                return BadRequest("Some technologies are not associated with the current user.");
            }

            // Format the project title using a helper function
            var formattedTitle = StringHelper.FormatTitleCase(projectDto.Title);

            // Validate URLs (Github and Deployed) using the helper function
            if (!string.IsNullOrWhiteSpace(projectDto.GithubLink) && !UrlHelper.IsValidUrl(projectDto.GithubLink))
            {
                return BadRequest("Invalid Github URL.");
            }

            if (!string.IsNullOrWhiteSpace(projectDto.DeployedLink) && !UrlHelper.IsValidUrl(projectDto.DeployedLink))
            {
                return BadRequest("Invalid Deployed URL.");
            }

            // Create the new project and associate it with the user
            var project = new Project
            {
                Title = formattedTitle,
                Description = projectDto.Description,
                GithubLink = projectDto.GithubLink,
                DeployedLink = projectDto.DeployedLink,
                IsVisible = projectDto.IsVisible,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
                Difficulty = (DifficultyLevel)projectDto.Difficulty,
                CategoryId = projectDto.CategoryId,
                UserId = userId // Link project to the current user
            };

            _context.Projects.Add(project);

            // Associate technologies with the project
            foreach (var tech in technologies)
            {
                _context.ProjectTechnologies.Add(new ProjectTechnology
                {
                    Project = project,
                    TechnologyId = tech.Id
                });
            }

            _context.SaveChanges();

            // Create a response DTO containing the project details and related data
            var projectWithDetails = new ProjectReadDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                GithubLink = project.GithubLink,
                DeployedLink = project.DeployedLink,
                IsVisible = project.IsVisible,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                DifficultyValue = (int)project.Difficulty,
                DifficultyName = project.Difficulty.ToString(),
                CategoryName = category.Name,
                Technologies = technologies.Select(t => new TechnologyReadDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    TechnologyGroupName = t.TechnologyGroup?.Name ?? "No Group" // Handle cases with no group
                }).ToList()
            };

            // Return the newly created project details
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectWithDetails);
        }

        // PUT /api/projects/{id} - Updates an existing project
        [HttpPut("{id}")]
        public IActionResult UpdateProject(int id, [FromBody] ProjectUpdateDto updatedProjectDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve the project if it belongs to the user
            var project = _context.Projects
                .Include(p => p.ProjectTechnologies) // Include related technologies
                .FirstOrDefault(p => p.Id == id && p.UserId == userId);

            if (project == null)
            {
                return NotFound();
            }

            // Ensure the difficulty level is valid
            if (!Enum.IsDefined(typeof(DifficultyLevel), updatedProjectDto.Difficulty))
            {
                return BadRequest("Invalid difficulty level.");
            }

            // Ensure that the user owns the Category
            var category = _context.UserCategories
                .Include(uc => uc.Category)
                .FirstOrDefault(uc => uc.CategoryId == updatedProjectDto.CategoryId && uc.UserId == userId)?.Category;

            if (category == null)
            {
                return BadRequest("The user is not associated with the specified category.");
            }

            // Ensure all technologies belong to the user
            var technologies = _context.Technologies
                .Include(t => t.UserTechnologies)
                .Where(t => updatedProjectDto.TechnologyIds.Contains(t.Id) && t.UserTechnologies.Any(ut => ut.UserId == userId))
                .Include(t => t.TechnologyGroup)
                .ToList();

            // If some technologies do not belong to the user, return an error
            if (technologies.Count != updatedProjectDto.TechnologyIds.Count)
            {
                return BadRequest("Some technologies are not associated with the current user.");
            }

            // Use helper to format the title
            var formattedTitle = StringHelper.FormatTitleCase(updatedProjectDto.Title);

            // Validate URLs using the helper function
            if (!string.IsNullOrWhiteSpace(updatedProjectDto.GithubLink) && !UrlHelper.IsValidUrl(updatedProjectDto.GithubLink))
            {
                return BadRequest("Invalid Github URL.");
            }

            if (!string.IsNullOrWhiteSpace(updatedProjectDto.DeployedLink) && !UrlHelper.IsValidUrl(updatedProjectDto.DeployedLink))
            {
                return BadRequest("Invalid Deployed URL.");
            }

            // Update the project details
            project.Title = formattedTitle;
            project.Description = updatedProjectDto.Description;
            project.GithubLink = updatedProjectDto.GithubLink;
            project.DeployedLink = updatedProjectDto.DeployedLink;
            project.IsVisible = updatedProjectDto.IsVisible;
            project.StartDate = updatedProjectDto.StartDate;
            project.EndDate = updatedProjectDto.EndDate;
            project.Difficulty = (DifficultyLevel)updatedProjectDto.Difficulty;
            project.CategoryId = updatedProjectDto.CategoryId;

            // Update project technologies
            _context.ProjectTechnologies.RemoveRange(project.ProjectTechnologies); // Remove old associations
            foreach (var tech in technologies)
            {
                _context.ProjectTechnologies.Add(new ProjectTechnology { ProjectId = id, TechnologyId = tech.Id });
            }

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE /api/projects/{id} - Deletes a project if it belongs to the authenticated user
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Ensure the project belongs to the current user
            var project = _context.Projects.FirstOrDefault(p => p.Id == id && p.UserId == userId);
            if (project == null)
            {
                return NotFound();
            }

            // Remove the project from the database
            _context.Projects.Remove(project);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
