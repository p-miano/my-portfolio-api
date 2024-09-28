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
            // Retrieve the current user's Id from the token (ClaimTypes.NameIdentifier)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if the user exists in the database
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Retrieve all categories associated with the current user
            var categories = _context.UserCategories
                .Where(uc => uc.UserId == user.Id) // Match categories by user Id
                .Include(uc => uc.Category) // Include the category details
                .Select(uc => new CategoryReadDto
                {
                    Id = uc.Category.Id,
                    Name = uc.Category.Name
                }).ToList(); // Map the categories to DTO

            return Ok(categories); // Return the list of categories
        }

        [Authorize]
        [HttpGet("{id}")] // Route: GET /api/categories/{id}
        public IActionResult GetCategory(int id)
        {
            // Retrieve the current user's Id
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if the user exists
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Retrieve the category by Id, ensuring the current user is associated with it
            var category = _context.UserCategories
                .Where(uc => uc.UserId == user.Id && uc.CategoryId == id)
                .Include(uc => uc.Category) // Include the category details
                .Select(uc => new CategoryReadDto
                {
                    Id = uc.Category.Id,
                    Name = uc.Category.Name
                })
                .FirstOrDefault(); // Return the category associated with the user

            if (category == null)
            {
                return NotFound(); // Return 404 if not found
            }

            return Ok(category); // Return the found category
        }

        [Authorize]
        [HttpPost] // Route: POST /api/categories
        public IActionResult CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            // Validate the incoming model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 if invalid
            }

            // Retrieve the current user's Id
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if the user exists
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Format the category name to title case
            var formattedName = StringHelper.FormatTitleCase(categoryDto.Name);

            // Check if the category already exists globally
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Name.ToLower() == formattedName.ToLower());

            if (existingCategory != null)
            {
                // Check if the user is already associated with this category
                var existingUserCategory = _context.UserCategories.FirstOrDefault(uc => uc.UserId == user.Id && uc.CategoryId == existingCategory.Id);
                if (existingUserCategory != null)
                {
                    return Conflict("Category already exists for this user."); // Return conflict if it already exists
                }

                // Create a new user-category association
                var newUserCategory = new UserCategory
                {
                    UserId = user.Id,
                    CategoryId = existingCategory.Id
                };

                // Save the association to the database
                _context.UserCategories.Add(newUserCategory);
                _context.SaveChanges();

                // Return the existing category
                return CreatedAtAction(nameof(GetCategory), new { id = existingCategory.Id }, new CategoryReadDto
                {
                    Id = existingCategory.Id,
                    Name = existingCategory.Name
                });
            }

            // Create a new category if it does not exist globally
            var newCategory = new Category
            {
                Name = formattedName
            };

            // Save the new category to the database
            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            // Create the user-category association
            var userCategory = new UserCategory
            {
                UserId = user.Id,
                CategoryId = newCategory.Id
            };

            // Save the association to the database
            _context.UserCategories.Add(userCategory);
            _context.SaveChanges();

            // Return the created category
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
                return NotFound("Category not found."); // Return 404 if category does not exist
            }

            // Ensure that the current user is associated with the category
            var userCategory = _context.UserCategories.FirstOrDefault(uc => uc.UserId == user.Id && uc.CategoryId == id);
            if (userCategory == null)
            {
                return Forbid("Bearer"); // Return 403 if the user has no permission to update
            }

            // Update the category name to the new value
            category.Name = StringHelper.FormatTitleCase(updatedCategoryDto.Name);

            // Save the changes to the database
            _context.SaveChanges();

            return NoContent(); // Return 204 on success with no content
        }

        [Authorize]
        [HttpDelete("{id}")] // Route: DELETE /api/categories/{id}
        public IActionResult DeleteCategory(int id)
        {
            // Retrieve the current user's Id
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if the user exists
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Find the category by Id
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            // Ensure the current user is associated with the category
            var userCategory = _context.UserCategories.FirstOrDefault(uc => uc.UserId == user.Id && uc.CategoryId == id);
            if (userCategory == null)
            {
                return Forbid("Bearer"); // Return 403 if user has no permission to delete
            }

            // Remove the association between the user and the category
            _context.UserCategories.Remove(userCategory);
            _context.SaveChanges();

            // If no other users are associated with this category, delete the category
            var isCategoryUsedByOthers = _context.UserCategories.Any(uc => uc.CategoryId == id);
            if (!isCategoryUsedByOthers)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }

            return NoContent(); // Return 204 on success
        }
    }
}
