using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoCICD.Persistence.Migrations;

/// <inheritdoc />
public partial class AddPositionEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Positions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Level = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Positions", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Position_Code",
            table: "Positions",
            column: "Code",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Position_Name",
            table: "Positions",
            column: "Name");

        // Add foreign key constraint from AppUsers to Positions
        migrationBuilder.CreateIndex(
            name: "IX_AppUsers_PositionId",
            table: "AppUsers",
            column: "PositionId");

        migrationBuilder.AddForeignKey(
            name: "FK_AppUsers_Positions_PositionId",
            table: "AppUsers",
            column: "PositionId",
            principalTable: "Positions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        // Insert some default positions
        migrationBuilder.InsertData(
            table: "Positions",
            columns: new[] { "Id", "Name", "Description", "Code", "Level", "IsActive", "CreatedAt" },
            values: new object[,]
            {
                { Guid.Parse("11111111-1111-1111-1111-111111111111"), "CEO", "Chief Executive Officer", "CEO", 1, true, DateTime.UtcNow },
                { Guid.Parse("22222222-2222-2222-2222-222222222222"), "CTO", "Chief Technology Officer", "CTO", 2, true, DateTime.UtcNow },
                { Guid.Parse("33333333-3333-3333-3333-333333333333"), "Manager", "Department Manager", "MANAGER", 3, true, DateTime.UtcNow },
                { Guid.Parse("44444444-4444-4444-4444-444444444444"), "Senior Developer", "Senior Software Developer", "SR_DEV", 4, true, DateTime.UtcNow },
                { Guid.Parse("55555555-5555-5555-5555-555555555555"), "Developer", "Software Developer", "DEVELOPER", 5, true, DateTime.UtcNow },
                { Guid.Parse("66666666-6666-6666-6666-666666666666"), "Junior Developer", "Junior Software Developer", "JR_DEV", 6, true, DateTime.UtcNow }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AppUsers_Positions_PositionId",
            table: "AppUsers");

        migrationBuilder.DropIndex(
            name: "IX_AppUsers_PositionId",
            table: "AppUsers");

        migrationBuilder.DropTable(
            name: "Positions");
    }
}