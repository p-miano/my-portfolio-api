using Microsoft.AspNetCore.Identity;

namespace my_portfolio_api.Models;

public class User : IdentityUser
{
    // Property to store the user's full name
    public string FullName { get; set; }

    // Navigation properties for many-to-many relationships
    public ICollection<UserCategory> UserCategories { get; set; }
    public ICollection<UserTechnology> UserTechnologies { get; set; }
    public ICollection<UserTechnologyGroup> UserTechnologyGroups { get; set; }
}