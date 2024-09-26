using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace my_portfolio_api.Models;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    // Add the navigation property for Projects
    [JsonIgnore]
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
