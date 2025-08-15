using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiceRolls.Migrations
{
    /// <inheritdoc />
    public partial class Throws_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Throw",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Login = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DiceType = table.Column<int>(type: "INTEGER", nullable: false),
                    DiceValues = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Throw", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Throw");
        }
    }
}
