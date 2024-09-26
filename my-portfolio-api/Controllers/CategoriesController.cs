using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_portfolio_api.Data;
using my_portfolio_api.DTOs;
using my_portfolio_api.Models;

namespace my_portfolio_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly PortfolioContext _context;

    public CategoriesController(PortfolioContext context)
    {
        _context = context;
    }

    // Route: GET /api/categories
    [HttpGet]
    public IActionResult GetCategories()
    {
        // Return a list of CategoryReadDto
        var categories = _context.Categories
            .Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

        return Ok(categories);
    }

    // Route: GET /api/categories/{id}
    [HttpGet("{id}")]
    public IActionResult GetCategory(int id)
    {
        // Return a single CategoryReadDto
        var category = _context.Categories
            .Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .FirstOrDefault(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    // Route: POST /api/categories
    [HttpPost]
    public IActionResult CreateCategory([FromBody] CategoryCreateDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Create a new category from the DTO
        var category = new Category
        {
            Name = categoryDto.Name
        };

        _context.Categories.Add(category);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, new CategoryReadDto
        {
            Id = category.Id,
            Name = category.Name
        });
    }

    // Route: PUT /api/categories/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateCategory(int id, [FromBody] CategoryUpdateDto updatedCategoryDto)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
        {
            return NotFound("Category not found.");
        }

        // Update the category based on the DTO
        category.Name = updatedCategoryDto.Name;
        _context.SaveChanges();

        return NoContent();
    }

    // Route: DELETE /api/categories/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteCategory(int id)
    {
        var category = _context.Categories
            .Include(c => c.Projects) // Include associated projects
            .FirstOrDefault(c => c.Id == id);

        if (category == null)
        {
            return NotFound("Category not found.");
        }

        if (category.Projects.Any()) // Check if any projects are associated
        {
            return BadRequest("Cannot delete category because it has associated projects.");
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();

        return NoContent();
    }
}
