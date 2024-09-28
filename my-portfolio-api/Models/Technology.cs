using my_portfolio_api.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace my_portfolio_api.Models;

public class Technology
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    // Foreign Key for TechnologyGroup
    public int TechnologyGroupId { get; set; }

    // Navigation Property for TechnologyGroup
    [Required]
    [JsonIgnore] // Avoid serializing TechnologyGroup to prevent cycles
    public TechnologyGroup TechnologyGroup { get; set; }

    // Navigation Property for Users
    [JsonIgnore]
    public ICollection<UserTechnology> UserTechnologies { get; set; } = new List<UserTechnology>();

    // Navigation Property for ProjectTechnologies
    [JsonIgnore]
    public ICollection<ProjectTechnology> ProjectTechnologies { get; set; } = new List<ProjectTechnology>();
}
