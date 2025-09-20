using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoCICD.Persistence.Migrations;

/// <inheritdoc />
public partial class InitialMigration_4 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Address",
            table: "AspNetUsers",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Avatar",
            table: "AspNetUsers",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Bio",
            table: "AspNetUsers",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "City",
            table: "AspNetUsers",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Country",
            table: "AspNetUsers",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Phone",
            table: "AspNetUsers",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Website",
            table: "AspNetUsers",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Address",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "Avatar",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "Bio",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "City",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "Country",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "Phone",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "Website",
            table: "AspNetUsers");
    }
}
