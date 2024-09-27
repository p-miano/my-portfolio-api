using my_portfolio_api.Models;
namespace my_portfolio_api.DTOs;

public class ProjectReadDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string GithubLink { get; set; }
    public string DeployedLink { get; set; }
    public bool IsVisible { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    // Return both the int and string name of the DifficultyLevel enum
    public int DifficultyValue { get; set; }
    public string DifficultyName { get; set; }
    public string CategoryName { get; set; }

    // Add this property to hold the technologies
    public List<TechnologyReadDto> Technologies { get; set; }
}


