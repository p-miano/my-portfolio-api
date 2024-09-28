namespace my_portfolio_api.Models;
using System.ComponentModel.DataAnnotations;

public class UserCategory
{
    [Required]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
