using Microsoft.AspNetCore.Mvc;
using my_portfolio_api.Data;
using my_portfolio_api.Models;

namespace my_portfolio_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TechnologyGroupsController : ControllerBase
{
    private readonly PortfolioContext _context;

    public TechnologyGroupsController(PortfolioContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetTechnologyGroups()
    {
        return Ok(_context.TechnologyGroups.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetTechnologyGroup(int id)
    {
        var group = _context.TechnologyGroups.Find(id);
        if (group == null)
        {
            return NotFound();
        }

        return Ok(group);
    }

    [HttpPost]
    public IActionResult CreateTechnologyGroup(TechnologyGroup group)
    {
        _context.TechnologyGroups.Add(group);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTechnologyGroup), new { id = group.Id }, group);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTechnologyGroup(int id, TechnologyGroup updatedGroup)
    {
        var group = _context.TechnologyGroups.Find(id);
        if (group == null)
        {
            return NotFound();
        }

        group.Name = updatedGroup.Name;
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTechnologyGroup(int id)
    {
        var group = _context.TechnologyGroups.Find(id);
        if (group == null)
        {
            return NotFound();
        }

        _context.TechnologyGroups.Remove(group);
        _context.SaveChanges();

        return NoContent();
    }
}
