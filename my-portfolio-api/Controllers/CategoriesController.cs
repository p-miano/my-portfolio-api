using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    [Route("api/categories")]
    public IActionResult GetCategories()
    {
        var categories = _context.Categories.Select(c => new
        {
            c.Id,
            c.Name
        }).ToList();

        return Ok(categories);
    }


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

    [HttpPost]
    public IActionResult CreateCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCategory(int id, Category updatedCategory)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }

        category.Name = updatedCategory.Name;
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();

        return NoContent();
    }
}
