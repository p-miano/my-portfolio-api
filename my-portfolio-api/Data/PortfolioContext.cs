using Microsoft.EntityFrameworkCore;
using my_portfolio_api.Models;

namespace my_portfolio_api.Data
{
    public class PortfolioContext : DbContext
    {
        public PortfolioContext(DbContextOptions<PortfolioContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<TechnologyGroup> TechnologyGroups { get; set; }
        public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Web" },
                new Category { Id = 2, Name = "Mobile" },
                new Category { Id = 3, Name = "Desktop" },
                new Category { Id = 4, Name = "API" }
            );

            // Seed data for Technology Groups
            modelBuilder.Entity<TechnologyGroup>().HasData(
                new TechnologyGroup { Id = 1, Name = "Programming Languages" },
                new TechnologyGroup { Id = 2, Name = "Frameworks" },
                new TechnologyGroup { Id = 3, Name = "Tools" }
            );

            // Seed data for Technologies
            modelBuilder.Entity<Technology>().HasData(
                new Technology { Id = 1, Name = "C#", TechnologyGroupId = 1 },
                new Technology { Id = 2, Name = "Java", TechnologyGroupId = 1 },
                new Technology { Id = 3, Name = "ASP.NET Core", TechnologyGroupId = 2 },
                new Technology { Id = 4, Name = "React", TechnologyGroupId = 2 }
            );

            // Seed data for Projects
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 1,
                    Title = "Portfolio Website",
                    Description = "Responsive website built with React and Bootstrap, featuring dynamic sections and showcasing my skills and projects.",
                    GithubLink = "https://github.com/p-miano/PaulaMiano.git",
                    DeployedLink = "https://p-miano.github.io/PaulaMiano",
                    IsVisible = true,
                    StartDate = new DateTime(2024, 09, 01),
                    EndDate = new DateTime(2024, 09, 25),
                    Difficulty = DifficultyLevel.Intermediate,
                    CategoryId = 1 // Web
                }
            );

            // Define the composite key for the ProjectTechnology entity
            modelBuilder.Entity<ProjectTechnology>()
                .HasKey(pt => new { pt.ProjectId, pt.TechnologyId });

            // Seed data for ProjectTechnology relationships
            modelBuilder.Entity<ProjectTechnology>().HasData(
                new ProjectTechnology { ProjectId = 1, TechnologyId = 1 }, // C#
                new ProjectTechnology { ProjectId = 1, TechnologyId = 3 }  // ASP.NET Core
            );
        }

    }
}
