namespace my_portfolio_api.DTOs;

public class TechnologyGroupReadDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<TechnologyReadDto> Technologies { get; set; } // Use TechnologyReadDto
}

