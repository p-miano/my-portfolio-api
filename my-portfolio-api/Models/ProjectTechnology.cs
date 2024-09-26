using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace my_portfolio_api.Models;

public class ProjectTechnology
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProjectId { get; set; }
    [JsonIgnore] // Avoid including full Project object in response
    [Required]
    public Project Project { get; set; }

    public int TechnologyId { get; set; }
    public Technology Technology { get; set; } // Keep Technology object but without recursion
}
