using Microsoft.AspNetCore.Identity;

namespace my_portfolio_api.Models;

public class ApplicationUser : IdentityUser
{
    // Property to store the user's full name
    public string FullName { get; set; }

    // Navigation property for many-to-many relationship with categories
    public ICollection<UserCategory> UserCategories { get; set; }
}