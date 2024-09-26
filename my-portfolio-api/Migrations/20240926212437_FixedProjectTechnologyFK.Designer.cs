﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using my_portfolio_api.Data;

#nullable disable

namespace my_portfolio_api.Migrations
{
    [DbContext(typeof(PortfolioContext))]
    [Migration("20240926212437_FixedProjectTechnologyFK")]
    partial class FixedProjectTechnologyFK
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("my_portfolio_api.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Web"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Mobile"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Desktop"
                        },
                        new
                        {
                            Id = 4,
                            Name = "API"
                        });
                });

            modelBuilder.Entity("my_portfolio_api.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeployedLink")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Difficulty")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("GithubLink")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Projects");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            DeployedLink = "https://p-miano.github.io/PaulaMiano",
                            Description = "Responsive website built with React and Bootstrap, featuring dynamic sections and showcasing my skills and projects.",
                            Difficulty = 2,
                            EndDate = new DateTime(2024, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GithubLink = "https://github.com/p-miano/PaulaMiano.git",
                            IsVisible = true,
                            StartDate = new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Portfolio Website"
                        });
                });

            modelBuilder.Entity("my_portfolio_api.Models.ProjectTechnology", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("INTEGER")
                        .HasColumnOrder(1);

                    b.Property<int>("TechnologyId")
                        .HasColumnType("INTEGER")
                        .HasColumnOrder(2);

                    b.HasKey("ProjectId", "TechnologyId");

                    b.HasIndex("TechnologyId");

                    b.ToTable("ProjectTechnologies");

                    b.HasData(
                        new
                        {
                            ProjectId = 1,
                            TechnologyId = 1
                        },
                        new
                        {
                            ProjectId = 1,
                            TechnologyId = 3
                        });
                });

            modelBuilder.Entity("my_portfolio_api.Models.Technology", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TechnologyGroupId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TechnologyGroupId");

                    b.ToTable("Technologies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "C#",
                            TechnologyGroupId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "Java",
                            TechnologyGroupId = 1
                        },
                        new
                        {
                            Id = 3,
                            Name = "ASP.NET Core",
                            TechnologyGroupId = 2
                        },
                        new
                        {
                            Id = 4,
                            Name = "React",
                            TechnologyGroupId = 2
                        });
                });

            modelBuilder.Entity("my_portfolio_api.Models.TechnologyGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TechnologyGroups");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Programming Languages"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Frameworks"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Tools"
                        });
                });

            modelBuilder.Entity("my_portfolio_api.Models.Project", b =>
                {
                    b.HasOne("my_portfolio_api.Models.Category", "Category")
                        .WithMany("Projects")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("my_portfolio_api.Models.ProjectTechnology", b =>
                {
                    b.HasOne("my_portfolio_api.Models.Project", "Project")
                        .WithMany("ProjectTechnologies")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("my_portfolio_api.Models.Technology", "Technology")
                        .WithMany("ProjectTechnologies")
                        .HasForeignKey("TechnologyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("Technology");
                });

            modelBuilder.Entity("my_portfolio_api.Models.Technology", b =>
                {
                    b.HasOne("my_portfolio_api.Models.TechnologyGroup", "TechnologyGroup")
                        .WithMany("Technologies")
                        .HasForeignKey("TechnologyGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TechnologyGroup");
                });

            modelBuilder.Entity("my_portfolio_api.Models.Category", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("my_portfolio_api.Models.Project", b =>
                {
                    b.Navigation("ProjectTechnologies");
                });

            modelBuilder.Entity("my_portfolio_api.Models.Technology", b =>
                {
                    b.Navigation("ProjectTechnologies");
                });

            modelBuilder.Entity("my_portfolio_api.Models.TechnologyGroup", b =>
                {
                    b.Navigation("Technologies");
                });
#pragma warning restore 612, 618
        }
    }
}
