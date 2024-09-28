using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace my_portfolio_api.Models;

public class ProjectTechnology
{
    // Composite Key: ProjectId and TechnologyId
    [Key, Column(Order = 1)]
    public int ProjectId { get; set; }

    [JsonIgnore] // Avoid including the entire Project object in the response
    [Required]
    public Project Project { get; set; }

    [Key, Column(Order = 2)]
    public int TechnologyId { get; set; }

    [JsonIgnore] // Avoid including the entire Technology object in the response
    [Required]
    public Technology Technology { get; set; }

}
