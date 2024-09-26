using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_portfolio_api.Data;
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
        var categories = _context.Categories.Select(c => new
        {
            c.Id,
            c.Name
        }).ToList();

        return Ok(categories);
    }

    // Route: GET /api/categories/{id}
    [HttpGet("{id}")]
    public IActionResult GetCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    // Route: POST /api/categories
    [HttpPost]
    public IActionResult CreateCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    // Route: PUT /api/categories/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateCategory(int id, [FromBody] Category updatedCategory)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
        {
            return NotFound("Category not found.");
        }

        category.Name = updatedCategory.Name;
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
