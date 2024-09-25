using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace my_portfolio_api.Models;

public class Project
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    public string? GithubLink { get; set; }
    public string? DeployedLink { get; set; }
    public bool IsVisible { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DifficultyLevel Difficulty { get; set; }

    // Foreign Key for Category
    public int CategoryId { get; set; }

    // Navigation Property for Category
    public Category Category { get; set; }

    // Navigation Property for ProjectTechnologies
    public ICollection<ProjectTechnology> ProjectTechnologies { get; set; }
}
