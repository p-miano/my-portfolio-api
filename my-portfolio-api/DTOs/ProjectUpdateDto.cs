using my_portfolio_api.Models;
using System.ComponentModel.DataAnnotations;
namespace my_portfolio_api.DTOs;

public class ProjectUpdateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string GithubLink { get; set; }
    public string DeployedLink { get; set; }
    public bool IsVisible { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    [EnumDataType(typeof(DifficultyLevel), ErrorMessage = "Invalid difficulty level.")]
    public int Difficulty { get; set; }

    public int CategoryId { get; set; }
    public List<int> TechnologyIds { get; set; }
}

