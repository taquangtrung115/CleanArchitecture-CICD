using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoCICD.Persistence.Migrations;

/// <inheritdoc />
public partial class AddChatEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ChatRooms",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Type = table.Column<int>(type: "int", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ChatRooms", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ChatMessages",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                Type = table.Column<int>(type: "int", nullable: false),
                IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ChatMessages", x => x.Id);
                table.ForeignKey(
                    name: "FK_ChatMessages_ChatRooms_RoomId",
                    column: x => x.RoomId,
                    principalTable: "ChatRooms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "ChatRoomMembers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Role = table.Column<int>(type: "int", nullable: false),
                JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastSeenDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ChatRoomMembers", x => x.Id);
                table.ForeignKey(
                    name: "FK_ChatRoomMembers_ChatRooms_RoomId",
                    column: x => x.RoomId,
                    principalTable: "ChatRooms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessages_CreatedAt",
            table: "ChatMessages",
            column: "CreatedAt");

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessages_IsRead",
            table: "ChatMessages",
            column: "IsRead");

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessages_ReceiverId",
            table: "ChatMessages",
            column: "ReceiverId");

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessages_RoomId",
            table: "ChatMessages",
            column: "RoomId");

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessages_RoomId_CreatedAt",
            table: "ChatMessages",
            columns: new[] { "RoomId", "CreatedAt" });

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessages_SenderId",
            table: "ChatMessages",
            column: "SenderId");

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessages_SenderId_ReceiverId",
            table: "ChatMessages",
            columns: new[] { "SenderId", "ReceiverId" });

        migrationBuilder.CreateIndex(
            name: "IX_ChatRoomMembers_IsActive",
            table: "ChatRoomMembers",
            column: "IsActive");

        migrationBuilder.CreateIndex(
            name: "IX_ChatRoomMembers_JoinedDate",
            table: "ChatRoomMembers",
            column: "JoinedDate");

        migrationBuilder.CreateIndex(
            name: "IX_ChatRoomMembers_RoomId",
            table: "ChatRoomMembers",
            column: "RoomId");

        migrationBuilder.CreateIndex(
            name: "IX_ChatRoomMembers_RoomId_UserId_Unique",
            table: "ChatRoomMembers",
            columns: new[] { "RoomId", "UserId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ChatRoomMembers_UserId",
            table: "ChatRoomMembers",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_ChatRooms_CreatedAt",
            table: "ChatRooms",
            column: "CreatedAt");

        migrationBuilder.CreateIndex(
            name: "IX_ChatRooms_IsActive",
            table: "ChatRooms",
            column: "IsActive");

        migrationBuilder.CreateIndex(
            name: "IX_ChatRooms_Name",
            table: "ChatRooms",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_ChatRooms_Type",
            table: "ChatRooms",
            column: "Type");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ChatMessages");

        migrationBuilder.DropTable(
            name: "ChatRoomMembers");

        migrationBuilder.DropTable(
            name: "ChatRooms");
    }
}
