using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace my_portfolio_api.Models;

public class ProjectTechnology
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProjectId { get; set; }
    [Required]
    public Project Project { get; set; }

    public int TechnologyId { get; set; }
    public Technology Technology { get; set; }
}
