using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace my_portfolio_api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAssociations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProjectTechnologies",
                keyColumns: new[] { "ProjectId", "TechnologyId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ProjectTechnologies",
                keyColumns: new[] { "ProjectId", "TechnologyId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "Technologies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Technologies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TechnologyGroups",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Technologies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Technologies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TechnologyGroups",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TechnologyGroups",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TechnologyGroups");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Technologies");

            migrationBuilder.CreateTable(
                name: "UserTechnologies",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    TechnologyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTechnologies", x => new { x.UserId, x.TechnologyId });
                    table.ForeignKey(
                        name: "FK_UserTechnologies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTechnologies_Technologies_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "Technologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTechnologyGroups",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    TechnologyGroupId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTechnologyGroups", x => new { x.UserId, x.TechnologyGroupId });
                    table.ForeignKey(
                        name: "FK_UserTechnologyGroups_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTechnologyGroups_TechnologyGroups_TechnologyGroupId",
                        column: x => x.TechnologyGroupId,
                        principalTable: "TechnologyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTechnologies_TechnologyId",
                table: "UserTechnologies",
                column: "TechnologyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTechnologyGroups_TechnologyGroupId",
                table: "UserTechnologyGroups",
                column: "TechnologyGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTechnologies");

            migrationBuilder.DropTable(
                name: "UserTechnologyGroups");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TechnologyGroups",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Technologies",
                type: "TEXT",
                nullable: true);

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
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "Programming Languages", null },
                    { 2, "Frameworks", null },
                    { 3, "Tools", null }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CategoryId", "DeployedLink", "Description", "Difficulty", "EndDate", "GithubLink", "IsVisible", "StartDate", "Title", "UserId" },
                values: new object[] { 1, 1, "https://p-miano.github.io/PaulaMiano", "Responsive website built with React and Bootstrap, featuring dynamic sections and showcasing my skills and projects.", 2, new DateTime(2024, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://github.com/p-miano/PaulaMiano.git", true, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Portfolio Website", null });

            migrationBuilder.InsertData(
                table: "Technologies",
                columns: new[] { "Id", "Name", "TechnologyGroupId", "UserId" },
                values: new object[,]
                {
                    { 1, "C#", 1, null },
                    { 2, "Java", 1, null },
                    { 3, "ASP.NET Core", 2, null },
                    { 4, "React", 2, null }
                });

            migrationBuilder.InsertData(
                table: "ProjectTechnologies",
                columns: new[] { "ProjectId", "TechnologyId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 }
                });
        }
    }
}
