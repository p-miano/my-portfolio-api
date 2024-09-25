using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace my_portfolio_api.Models;

public class TechnologyGroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }

    // Navigation Property for Technologies in this group
    public ICollection<Technology> Technologies { get; set; }
}
