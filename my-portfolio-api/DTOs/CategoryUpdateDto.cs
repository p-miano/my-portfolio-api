using System.ComponentModel.DataAnnotations;

namespace my_portfolio_api.DTOs
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 50 characters.")]
        public string Name { get; set; }
    }

}
