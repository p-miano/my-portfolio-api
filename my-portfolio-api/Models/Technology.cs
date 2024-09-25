using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
    public TechnologyGroup TechnologyGroup { get; set; }

    // Navigation Property for ProjectTechnologies
    public ICollection<ProjectTechnology> ProjectTechnologies { get; set; }
}
