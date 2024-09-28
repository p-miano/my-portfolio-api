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
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly PortfolioContext _context;

        public CategoriesController(PortfolioContext context)
        {
            _context = context;
        }
        
        [Authorize]
        [HttpGet] // Route: GET /api/categories
        public IActionResult GetCategories()
        {
            // Retrieve the current user's Id from token (ClaimTypes.NameIdentifier)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if the user exists
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Get categories associated with the current user
            var categories = _context.UserCategories
                .Where(uc => uc.UserId == user.Id)
                .Include(uc => uc.Category)
                .Select(uc => new CategoryReadDto
                {
                    Id = uc.Category.Id,
                    Name = uc.Category.Name
                }).ToList();

            return Ok(categories);
        }
        
        [Authorize]
        [HttpGet("{id}")] // Route: GET /api/categories/{id}
        public IActionResult GetCategory(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var category = _context.UserCategories
                .Where(uc => uc.UserId == user.Id && uc.CategoryId == id)
                .Include(uc => uc.Category)
                .Select(uc => new CategoryReadDto
                {
                    Id = uc.Category.Id,
                    Name = uc.Category.Name
                })
                .FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        
        [Authorize]
        [HttpPost] // Route: POST /api/categories
        public IActionResult CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the userId from the token (ClaimTypes.NameIdentifier)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve the user from the database
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var formattedName = StringHelper.FormatTitleCase(categoryDto.Name);
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Name.ToLower() == formattedName.ToLower());

            if (existingCategory != null)
            {
                var existingUserCategory = _context.UserCategories.FirstOrDefault(uc => uc.UserId == user.Id && uc.CategoryId == existingCategory.Id);
                if (existingUserCategory != null)
                {
                    return Conflict("Category already exists for this user.");
                }

                var newUserCategory = new UserCategory
                {
                    UserId = user.Id,
                    CategoryId = existingCategory.Id
                };

                _context.UserCategories.Add(newUserCategory);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetCategory), new { id = existingCategory.Id }, new CategoryReadDto
                {
                    Id = existingCategory.Id,
                    Name = existingCategory.Name
                });
            }

            var newCategory = new Category
            {
                Name = formattedName
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            var userCategory = new UserCategory
            {
                UserId = user.Id,
                CategoryId = newCategory.Id
            };

            _context.UserCategories.Add(userCategory);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategory), new { id = newCategory.Id }, new CategoryReadDto
            {
                Id = newCategory.Id,
                Name = newCategory.Name
            });
        }
        
        [Authorize]
        [HttpPut("{id}")] // Route: PUT /api/categories/{id}
        public IActionResult UpdateCategory(int id, [FromBody] CategoryUpdateDto updatedCategoryDto)
        {
            // Retrieve the current user's Id
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if the user exists
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Check if the category exists
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            // Ensure that the current user is associated with the category
            var userCategory = _context.UserCategories.FirstOrDefault(uc => uc.UserId == user.Id && uc.CategoryId == id);
            if (userCategory == null)
            {
                return Forbid("Bearer");
            }

            // Update the category name
            category.Name = StringHelper.FormatTitleCase(updatedCategoryDto.Name);

            // Save the changes
            _context.SaveChanges();

            return NoContent();
        }
                
        [Authorize]
        [HttpDelete("{id}")] // Route: DELETE /api/categories/{id}
        public IActionResult DeleteCategory(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            var userCategory = _context.UserCategories.FirstOrDefault(uc => uc.UserId == user.Id && uc.CategoryId == id);
            if (userCategory == null)
            {
                return Forbid("Bearer");
            }

            _context.UserCategories.Remove(userCategory);
            _context.SaveChanges();

            var isCategoryUsedByOthers = _context.UserCategories.Any(uc => uc.CategoryId == id);
            if (!isCategoryUsedByOthers)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }

            return NoContent();
        }
    }
}
