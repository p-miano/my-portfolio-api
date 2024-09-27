namespace my_portfolio_api.DTOs;

public class TechnologyGroupReadDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<TechnologyReadNoGroupDto> Technologies { get; set; } // Use TechnologyReadDto
}

