using System.ComponentModel.DataAnnotations;

namespace my_portfolio_api.DTOs;

public class TechnologyCreateDto
{
    [Required(ErrorMessage = "Technology name is required.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Technology Group is required and cannot be null.")]
    public int TechnologyGroupId { get; set; }
}
