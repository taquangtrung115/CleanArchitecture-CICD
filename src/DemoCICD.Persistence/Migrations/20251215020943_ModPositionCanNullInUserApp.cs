using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoCICD.Persistence.Migrations;

/// <inheritdoc />
public partial class ModPositionCanNullInUserApp : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "PositionId",
            table: "AspNetUsers",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddColumn<string>(
            name: "FunctionId1",
            table: "ActionInFunctions",
            type: "nvarchar(50)",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_ActionInFunctions_FunctionId1",
            table: "ActionInFunctions",
            column: "FunctionId1");

        migrationBuilder.AddForeignKey(
            name: "FK_ActionInFunctions_Functions_FunctionId1",
            table: "ActionInFunctions",
            column: "FunctionId1",
            principalTable: "Functions",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ActionInFunctions_Functions_FunctionId1",
            table: "ActionInFunctions");

        migrationBuilder.DropIndex(
            name: "IX_ActionInFunctions_FunctionId1",
            table: "ActionInFunctions");

        migrationBuilder.DropColumn(
            name: "FunctionId1",
            table: "ActionInFunctions");

        migrationBuilder.AlterColumn<Guid>(
            name: "PositionId",
            table: "AspNetUsers",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);
    }
}
