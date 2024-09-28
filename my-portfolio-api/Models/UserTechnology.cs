using System.ComponentModel.DataAnnotations;

namespace my_portfolio_api.Models;

public class UserTechnology
{
    [Key]
    [Required]
    public string UserId { get; set; }
    public User User { get; set; }

    [Key]
    [Required]
    public int TechnologyId { get; set; }
    public Technology Technology { get; set; }
}
