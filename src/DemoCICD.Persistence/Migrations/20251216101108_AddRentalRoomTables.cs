using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoCICD.Persistence.Migrations;

/// <inheritdoc />
public partial class AddRentalRoomTables : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Locations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Ward = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                FullAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                Latitude = table.Column<double>(type: "float", nullable: true),
                Longitude = table.Column<double>(type: "float", nullable: true),
                Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Locations", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Rooms",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoomNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Capacity = table.Column<int>(type: "int", nullable: false),
                PricePerNight = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rooms", x => x.Id);
                table.ForeignKey(
                    name: "FK_Rooms_Locations_LocationId",
                    column: x => x.LocationId,
                    principalTable: "Locations",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Profiles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                IdentityCard = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                PermanentAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Occupation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                RentStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                RentEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                DepositAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Profiles", x => x.Id);
                table.ForeignKey(
                    name: "FK_Profiles_Rooms_RoomId",
                    column: x => x.RoomId,
                    principalTable: "Rooms",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Bills",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BillNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Month = table.Column<int>(type: "int", nullable: false),
                Year = table.Column<int>(type: "int", nullable: false),
                IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                RoomPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                ServiceTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                PaidAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Bills", x => x.Id);
                table.ForeignKey(
                    name: "FK_Bills_Profiles_ProfileId",
                    column: x => x.ProfileId,
                    principalTable: "Profiles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Bills_Rooms_RoomId",
                    column: x => x.RoomId,
                    principalTable: "Rooms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "BillDetails",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ServiceType = table.Column<int>(type: "int", nullable: false),
                ServiceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                OldIndex = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                NewIndex = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                BillId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BillDetails", x => x.Id);
                table.ForeignKey(
                    name: "FK_BillDetails_Bills_BillId",
                    column: x => x.BillId,
                    principalTable: "Bills",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_BillDetails_Bills_BillId1",
                    column: x => x.BillId1,
                    principalTable: "Bills",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_BillDetails_BillId",
            table: "BillDetails",
            column: "BillId");

        migrationBuilder.CreateIndex(
            name: "IX_BillDetails_BillId1",
            table: "BillDetails",
            column: "BillId1");

        migrationBuilder.CreateIndex(
            name: "IX_BillDetails_ServiceType",
            table: "BillDetails",
            column: "ServiceType");

        migrationBuilder.CreateIndex(
            name: "IX_Bills_BillNumber",
            table: "Bills",
            column: "BillNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Bills_Month_Year",
            table: "Bills",
            columns: new[] { "Month", "Year" });

        migrationBuilder.CreateIndex(
            name: "IX_Bills_ProfileId",
            table: "Bills",
            column: "ProfileId");

        migrationBuilder.CreateIndex(
            name: "IX_Bills_RoomId",
            table: "Bills",
            column: "RoomId");

        migrationBuilder.CreateIndex(
            name: "IX_Bills_Status",
            table: "Bills",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "IX_Locations_City",
            table: "Locations",
            column: "City");

        migrationBuilder.CreateIndex(
            name: "IX_Locations_District",
            table: "Locations",
            column: "District");

        migrationBuilder.CreateIndex(
            name: "IX_Profiles_IdentityCard",
            table: "Profiles",
            column: "IdentityCard",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Profiles_IsActive",
            table: "Profiles",
            column: "IsActive");

        migrationBuilder.CreateIndex(
            name: "IX_Profiles_PhoneNumber",
            table: "Profiles",
            column: "PhoneNumber");

        migrationBuilder.CreateIndex(
            name: "IX_Profiles_RoomId",
            table: "Profiles",
            column: "RoomId");

        migrationBuilder.CreateIndex(
            name: "IX_Rooms_IsAvailable",
            table: "Rooms",
            column: "IsAvailable");

        migrationBuilder.CreateIndex(
            name: "IX_Rooms_LocationId",
            table: "Rooms",
            column: "LocationId");

        migrationBuilder.CreateIndex(
            name: "IX_Rooms_RoomNumber",
            table: "Rooms",
            column: "RoomNumber",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "BillDetails");

        migrationBuilder.DropTable(
            name: "Bills");

        migrationBuilder.DropTable(
            name: "Profiles");

        migrationBuilder.DropTable(
            name: "Rooms");

        migrationBuilder.DropTable(
            name: "Locations");
    }
}
