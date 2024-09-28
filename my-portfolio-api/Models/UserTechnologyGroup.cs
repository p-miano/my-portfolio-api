using System.ComponentModel.DataAnnotations;

namespace my_portfolio_api.Models;

public class UserTechnologyGroup
{
    [Key]
    [Required]
    public string UserId { get; set; }
    public User User { get; set; }

    [Key]
    [Required]
    public int TechnologyGroupId { get; set; }
    public TechnologyGroup TechnologyGroup { get; set; }
}

