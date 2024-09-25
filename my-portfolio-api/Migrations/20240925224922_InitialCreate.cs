using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace my_portfolio_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnologyGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologyGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    GithubLink = table.Column<string>(type: "TEXT", nullable: true),
                    DeployedLink = table.Column<string>(type: "TEXT", nullable: true),
                    IsVisible = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Difficulty = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Technologies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TechnologyGroupId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technologies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Technologies_TechnologyGroups_TechnologyGroupId",
                        column: x => x.TechnologyGroupId,
                        principalTable: "TechnologyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTechnologies",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    TechnologyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTechnologies", x => new { x.ProjectId, x.TechnologyId });
                    table.ForeignKey(
                        name: "FK_ProjectTechnologies_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTechnologies_Technologies_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "Technologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Web" },
                    { 2, "Mobile" },
                    { 3, "Desktop" },
                    { 4, "API" }
                });

            migrationBuilder.InsertData(
                table: "TechnologyGroups",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Programming Languages" },
                    { 2, "Frameworks" },
                    { 3, "Tools" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CategoryId", "DeployedLink", "Description", "Difficulty", "EndDate", "GithubLink", "IsVisible", "StartDate", "Title" },
                values: new object[] { 1, 1, "https://p-miano.github.io/PaulaMiano", "Responsive website built with React and Bootstrap, featuring dynamic sections and showcasing my skills and projects.", 2, new DateTime(2024, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://github.com/p-miano/PaulaMiano.git", true, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Portfolio Website" });

            migrationBuilder.InsertData(
                table: "Technologies",
                columns: new[] { "Id", "Name", "TechnologyGroupId" },
                values: new object[,]
                {
                    { 1, "C#", 1 },
                    { 2, "Java", 1 },
                    { 3, "ASP.NET Core", 2 },
                    { 4, "React", 2 }
                });

            migrationBuilder.InsertData(
                table: "ProjectTechnologies",
                columns: new[] { "ProjectId", "TechnologyId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CategoryId",
                table: "Projects",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTechnologies_TechnologyId",
                table: "ProjectTechnologies",
                column: "TechnologyId");

            migrationBuilder.CreateIndex(
                name: "IX_Technologies_TechnologyGroupId",
                table: "Technologies",
                column: "TechnologyGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTechnologies");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Technologies");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "TechnologyGroups");
        }
    }
}
