using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using my_portfolio_api.Models;

namespace my_portfolio_api.Data
{
    public class PortfolioContext : IdentityDbContext<User>
    {
        public PortfolioContext(DbContextOptions<PortfolioContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<UserCategory> UserCategories { get; set; } // Junction table for many-to-many relationship
        public DbSet<Project> Projects { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<UserTechnology> UserTechnologies { get; set; } // Junction table for many-to-many relationship
        public DbSet<TechnologyGroup> TechnologyGroups { get; set; }
        public DbSet<UserTechnologyGroup> UserTechnologyGroups { get; set; } // Junction table for many-to-many relationship
        public DbSet<ProjectTechnology> ProjectTechnologies { get; set; } // Junction table for many-to-many relationship

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship between User and Category via UserCategory
            modelBuilder.Entity<UserCategory>()
                .HasKey(uc => new { uc.UserId, uc.CategoryId }); // Composite key

            modelBuilder.Entity<UserCategory>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCategories)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade on delete

            modelBuilder.Entity<UserCategory>()
                .HasOne(uc => uc.Category)
                .WithMany(c => c.UserCategories)
                .HasForeignKey(uc => uc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade on delete

            // Configure many-to-many relationship between User and Technology via UserTechnology
            modelBuilder.Entity<UserTechnology>()
                .HasKey(ut => new { ut.UserId, ut.TechnologyId }); // Composite key

            modelBuilder.Entity<UserTechnology>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTechnologies)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade on delete

            modelBuilder.Entity<UserTechnology>()
                .HasOne(ut => ut.Technology)
                .WithMany(t => t.UserTechnologies)
                .HasForeignKey(ut => ut.TechnologyId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade on delete

            // Configure many-to-many relationship between User and TechnologyGroup via UserTechnologyGroup
            modelBuilder.Entity<UserTechnologyGroup>()
                .HasKey(utg => new { utg.UserId, utg.TechnologyGroupId }); // Composite key

            modelBuilder.Entity<UserTechnologyGroup>()
                .HasOne(utg => utg.User)
                .WithMany(u => u.UserTechnologyGroups)
                .HasForeignKey(utg => utg.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade on delete

            modelBuilder.Entity<UserTechnologyGroup>()
                .HasOne(utg => utg.TechnologyGroup)
                .WithMany(tg => tg.UserTechnologyGroups)
                .HasForeignKey(utg => utg.TechnologyGroupId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade on delete

            // Define the composite key for ProjectTechnology (many-to-many relationship between Projects and Technologies)
            modelBuilder.Entity<ProjectTechnology>()
                .HasKey(pt => new { pt.ProjectId, pt.TechnologyId });

            //// Seed data for Categories
            //modelBuilder.Entity<Category>().HasData(
            //    new Category { Id = 1, Name = "Web" },
            //    new Category { Id = 2, Name = "Mobile" },
            //    new Category { Id = 3, Name = "Desktop" },
            //    new Category { Id = 4, Name = "API" }
            //);

            //// Seed data for Technology Groups
            //modelBuilder.Entity<TechnologyGroup>().HasData(
            //    new TechnologyGroup { Id = 1, Name = "Programming Languages" },
            //    new TechnologyGroup { Id = 2, Name = "Frameworks" },
            //    new TechnologyGroup { Id = 3, Name = "Tools" }
            //);

            //// Seed data for Technologies
            //modelBuilder.Entity<Technology>().HasData(
            //    new Technology { Id = 1, Name = "C#", TechnologyGroupId = 1 },
            //    new Technology { Id = 2, Name = "Java", TechnologyGroupId = 1 },
            //    new Technology { Id = 3, Name = "ASP.NET Core", TechnologyGroupId = 2 },
            //    new Technology { Id = 4, Name = "React", TechnologyGroupId = 2 }
            //);

            //// Seed data for Projects
            //modelBuilder.Entity<Project>().HasData(
            //    new Project
            //    {
            //        Id = 1,
            //        Title = "Portfolio Website",
            //        Description = "Responsive website built with React and Bootstrap, featuring dynamic sections and showcasing my skills and projects.",
            //        GithubLink = "https://github.com/p-miano/PaulaMiano.git",
            //        DeployedLink = "https://p-miano.github.io/PaulaMiano",
            //        IsVisible = true,
            //        StartDate = new DateTime(2024, 09, 01),
            //        EndDate = new DateTime(2024, 09, 25),
            //        Difficulty = DifficultyLevel.Intermediate,
            //        CategoryId = 1 // Web
            //    }
            //);

            //// Seed data for ProjectTechnology relationships
            //modelBuilder.Entity<ProjectTechnology>().HasData(
            //    new ProjectTechnology { ProjectId = 1, TechnologyId = 1 }, // C#
            //    new ProjectTechnology { ProjectId = 1, TechnologyId = 3 }  // ASP.NET Core
            //);
        }
    }
}
