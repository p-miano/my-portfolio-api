using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace my_portfolio_api.Models;

public class TechnologyGroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    // Navigation Property for Technologies in this group    
    [JsonIgnore] // Avoid serializing Technologies to prevent cycles
    public ICollection<Technology> Technologies { get; set; }

    // Navigation Property for Users
    [JsonIgnore]
    public ICollection<UserTechnologyGroup> UserTechnologyGroups { get; set; } = new List<UserTechnologyGroup>();
}
