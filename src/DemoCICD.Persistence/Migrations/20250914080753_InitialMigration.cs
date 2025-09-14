using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoCICD.Persistence.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Actions",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                SortOrder = table.Column<int>(type: "int", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Actions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                RoleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUsers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DayOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDirector = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                IsHeadOfDepartment = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                PositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsReceipient = table.Column<int>(type: "int", nullable: false, defaultValue: -1),
                UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Functions",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Url = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ParrentId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                SortOrder = table.Column<int>(type: "int", nullable: true),
                CssClass = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Functions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "News",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Summary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                FeaturedImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                ImageCaption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Slug = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                IsBreaking = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                RelatedSeasonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                RelatedRaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                RelatedTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                RelatedRiderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Tags = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_News", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Product",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Product", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Seasons",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Year = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                IsCurrentSeason = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Seasons", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Teams",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                ShortName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                CountryCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                CountryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                CountryFlag = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Logo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Website = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                FoundedYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                PrimaryColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                SecondaryColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Teams", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Videos",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                ThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                ExternalVideoId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                Platform = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                RelatedSeasonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                RelatedRaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                RelatedTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                RelatedRiderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Tags = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Videos", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_AspNetRoles_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_AspNetUsers_AppUserId",
                    column: x => x.AppUserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserLogins",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_AspNetUserLogins_AspNetUsers_AppUserId",
                    column: x => x.AppUserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserRoles",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AppRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetRoles_AppRoleId",
                    column: x => x.AppRoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetUsers_AppUserId",
                    column: x => x.AppUserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserTokens",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_AspNetUserTokens_AspNetUsers_AppUserId",
                    column: x => x.AppUserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ActionInFunctions",
            columns: table => new
            {
                ActionId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                FunctionId = table.Column<string>(type: "nvarchar(50)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ActionInFunctions", x => new { x.ActionId, x.FunctionId });
                table.ForeignKey(
                    name: "FK_ActionInFunctions_Actions_ActionId",
                    column: x => x.ActionId,
                    principalTable: "Actions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ActionInFunctions_Functions_FunctionId",
                    column: x => x.FunctionId,
                    principalTable: "Functions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Permissions",
            columns: table => new
            {
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FunctionId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                ActionId = table.Column<string>(type: "nvarchar(50)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Permissions", x => new { x.RoleId, x.FunctionId, x.ActionId });
                table.ForeignKey(
                    name: "FK_Permissions_Actions_ActionId",
                    column: x => x.ActionId,
                    principalTable: "Actions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Permissions_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Permissions_Functions_FunctionId",
                    column: x => x.FunctionId,
                    principalTable: "Functions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Races",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SeasonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                CircuitName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                CountryCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                CountryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                CountryFlag = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                RaceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Practice1DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                Practice2DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                Practice3DateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                QualifyingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                SprintQualifyingDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                SprintRaceDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                RaceDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                RoundNumber = table.Column<int>(type: "int", nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                WeatherConditions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                CircuitLength = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                NumberOfLaps = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Races", x => x.Id);
                table.ForeignKey(
                    name: "FK_Races_Seasons_SeasonId",
                    column: x => x.SeasonId,
                    principalTable: "Seasons",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Bikes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Year = table.Column<int>(type: "int", nullable: false),
                TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SpecEngine = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                SpecDisplacement = table.Column<int>(type: "int", nullable: false),
                SpecMaxPower = table.Column<int>(type: "int", nullable: false),
                SpecMaxSpeed = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                SpecWeight = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                SpecTransmission = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                SpecFrame = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                SpecFrontSuspension = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                SpecRearSuspension = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                SpecFrontBrakes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                SpecRearBrakes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                ChassisNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                EngineNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Livery = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Bikes", x => x.Id);
                table.ForeignKey(
                    name: "FK_Bikes_Teams_TeamId",
                    column: x => x.TeamId,
                    principalTable: "Teams",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "Riders",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                RacingNumber = table.Column<int>(type: "int", nullable: false),
                NationalityCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                NationalityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                NationalityFlag = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                CurrentTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Nickname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Photo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Height = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Weight = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                DebutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                RetirementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Riders", x => x.Id);
                table.ForeignKey(
                    name: "FK_Riders_Teams_CurrentTeamId",
                    column: x => x.CurrentTeamId,
                    principalTable: "Teams",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "RaceEntries",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RiderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BikeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                StartingGrid = table.Column<int>(type: "int", nullable: false),
                ResultPosition = table.Column<int>(type: "int", nullable: true),
                ResultPoints = table.Column<int>(type: "int", nullable: true),
                ResultRaceTime = table.Column<TimeSpan>(type: "time", nullable: true),
                ResultGapToWinner = table.Column<TimeSpan>(type: "time", nullable: true),
                ResultDidNotFinish = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                ResultDidNotStart = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                ResultDisqualified = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                ResultReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                FastestLap = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                FastestLapTime = table.Column<TimeSpan>(type: "time", nullable: true),
                Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RaceEntries", x => x.Id);
                table.ForeignKey(
                    name: "FK_RaceEntries_Races_RaceId",
                    column: x => x.RaceId,
                    principalTable: "Races",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "RiderTeamHistories",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RiderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SeasonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RiderTeamHistories", x => x.Id);
                table.ForeignKey(
                    name: "FK_RiderTeamHistories_Riders_RiderId",
                    column: x => x.RiderId,
                    principalTable: "Riders",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ActionInFunctions_FunctionId",
            table: "ActionInFunctions",
            column: "FunctionId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetRoleClaims_RoleId",
            table: "AspNetRoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "AspNetRoles",
            column: "NormalizedName",
            unique: true,
            filter: "[NormalizedName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserClaims_AppUserId",
            table: "AspNetUserClaims",
            column: "AppUserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserClaims_UserId",
            table: "AspNetUserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserLogins_AppUserId",
            table: "AspNetUserLogins",
            column: "AppUserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserLogins_UserId",
            table: "AspNetUserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserRoles_AppRoleId",
            table: "AspNetUserRoles",
            column: "AppRoleId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserRoles_AppUserId",
            table: "AspNetUserRoles",
            column: "AppUserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserRoles_RoleId",
            table: "AspNetUserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "AspNetUsers",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "AspNetUsers",
            column: "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserTokens_AppUserId",
            table: "AspNetUserTokens",
            column: "AppUserId");

        migrationBuilder.CreateIndex(
            name: "IX_Bikes_ChassisNumber",
            table: "Bikes",
            column: "ChassisNumber",
            unique: true,
            filter: "ChassisNumber IS NOT NULL AND IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Bikes_EngineNumber",
            table: "Bikes",
            column: "EngineNumber",
            unique: true,
            filter: "EngineNumber IS NOT NULL AND IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Bikes_IsActive",
            table: "Bikes",
            column: "IsActive");

        migrationBuilder.CreateIndex(
            name: "IX_Bikes_IsDeleted",
            table: "Bikes",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_Bikes_Manufacturer",
            table: "Bikes",
            column: "Manufacturer");

        migrationBuilder.CreateIndex(
            name: "IX_Bikes_TeamId",
            table: "Bikes",
            column: "TeamId");

        migrationBuilder.CreateIndex(
            name: "IX_Bikes_Year",
            table: "Bikes",
            column: "Year");

        migrationBuilder.CreateIndex(
            name: "IX_News_AuthorId",
            table: "News",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_News_Category",
            table: "News",
            column: "Category");

        migrationBuilder.CreateIndex(
            name: "IX_News_IsBreaking",
            table: "News",
            column: "IsBreaking",
            filter: "IsBreaking = 1 AND Status = 'Published' AND IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_News_IsDeleted",
            table: "News",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_News_IsFeatured",
            table: "News",
            column: "IsFeatured",
            filter: "IsFeatured = 1 AND Status = 'Published' AND IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_News_PublishedDate",
            table: "News",
            column: "PublishedDate",
            filter: "Status = 'Published' AND IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_News_RelatedRaceId",
            table: "News",
            column: "RelatedRaceId");

        migrationBuilder.CreateIndex(
            name: "IX_News_RelatedRiderId",
            table: "News",
            column: "RelatedRiderId");

        migrationBuilder.CreateIndex(
            name: "IX_News_RelatedSeasonId",
            table: "News",
            column: "RelatedSeasonId");

        migrationBuilder.CreateIndex(
            name: "IX_News_RelatedTeamId",
            table: "News",
            column: "RelatedTeamId");

        migrationBuilder.CreateIndex(
            name: "IX_News_Slug",
            table: "News",
            column: "Slug",
            unique: true,
            filter: "IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_News_Status",
            table: "News",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "IX_Permissions_ActionId",
            table: "Permissions",
            column: "ActionId");

        migrationBuilder.CreateIndex(
            name: "IX_Permissions_FunctionId",
            table: "Permissions",
            column: "FunctionId");

        migrationBuilder.CreateIndex(
            name: "IX_RaceEntries_BikeId",
            table: "RaceEntries",
            column: "BikeId");

        migrationBuilder.CreateIndex(
            name: "IX_RaceEntries_FastestLap",
            table: "RaceEntries",
            column: "FastestLap",
            filter: "FastestLap = 1 AND IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_RaceEntries_IsDeleted",
            table: "RaceEntries",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_RaceEntries_RaceId",
            table: "RaceEntries",
            column: "RaceId");

        migrationBuilder.CreateIndex(
            name: "IX_RaceEntries_RaceId_RiderId",
            table: "RaceEntries",
            columns: new[] { "RaceId", "RiderId" },
            unique: true,
            filter: "IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_RaceEntries_RaceId_StartingGrid",
            table: "RaceEntries",
            columns: new[] { "RaceId", "StartingGrid" },
            unique: true,
            filter: "IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_RaceEntries_RiderId",
            table: "RaceEntries",
            column: "RiderId");

        migrationBuilder.CreateIndex(
            name: "IX_RaceEntries_TeamId",
            table: "RaceEntries",
            column: "TeamId");

        migrationBuilder.CreateIndex(
            name: "IX_Races_IsDeleted",
            table: "Races",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_Races_RaceDate",
            table: "Races",
            column: "RaceDate");

        migrationBuilder.CreateIndex(
            name: "IX_Races_SeasonId",
            table: "Races",
            column: "SeasonId");

        migrationBuilder.CreateIndex(
            name: "IX_Races_SeasonId_RoundNumber",
            table: "Races",
            columns: new[] { "SeasonId", "RoundNumber" },
            unique: true,
            filter: "IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Races_Status",
            table: "Races",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "IX_Riders_CurrentTeamId",
            table: "Riders",
            column: "CurrentTeamId");

        migrationBuilder.CreateIndex(
            name: "IX_Riders_IsActive",
            table: "Riders",
            column: "IsActive");

        migrationBuilder.CreateIndex(
            name: "IX_Riders_IsDeleted",
            table: "Riders",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_Riders_LastName_FirstName",
            table: "Riders",
            columns: new[] { "LastName", "FirstName" });

        migrationBuilder.CreateIndex(
            name: "IX_Riders_RacingNumber",
            table: "Riders",
            column: "RacingNumber",
            unique: true,
            filter: "IsActive = 1 AND IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_RiderTeamHistories_EndDate",
            table: "RiderTeamHistories",
            column: "EndDate",
            filter: "EndDate IS NULL AND IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_RiderTeamHistories_IsDeleted",
            table: "RiderTeamHistories",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_RiderTeamHistories_RiderId",
            table: "RiderTeamHistories",
            column: "RiderId");

        migrationBuilder.CreateIndex(
            name: "IX_RiderTeamHistories_RiderId_StartDate",
            table: "RiderTeamHistories",
            columns: new[] { "RiderId", "StartDate" });

        migrationBuilder.CreateIndex(
            name: "IX_RiderTeamHistories_SeasonId",
            table: "RiderTeamHistories",
            column: "SeasonId");

        migrationBuilder.CreateIndex(
            name: "IX_RiderTeamHistories_TeamId",
            table: "RiderTeamHistories",
            column: "TeamId");

        migrationBuilder.CreateIndex(
            name: "IX_Seasons_IsCurrentSeason",
            table: "Seasons",
            column: "IsCurrentSeason",
            filter: "IsCurrentSeason = 1 AND IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Seasons_IsDeleted",
            table: "Seasons",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_Seasons_Year",
            table: "Seasons",
            column: "Year",
            unique: true,
            filter: "IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Teams_IsActive",
            table: "Teams",
            column: "IsActive");

        migrationBuilder.CreateIndex(
            name: "IX_Teams_IsDeleted",
            table: "Teams",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_Teams_Name",
            table: "Teams",
            column: "Name",
            unique: true,
            filter: "IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Teams_ShortName",
            table: "Teams",
            column: "ShortName",
            unique: true,
            filter: "IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Videos_ExternalVideoId",
            table: "Videos",
            column: "ExternalVideoId",
            filter: "ExternalVideoId IS NOT NULL AND IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Videos_IsDeleted",
            table: "Videos",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_Videos_IsFeatured",
            table: "Videos",
            column: "IsFeatured",
            filter: "IsFeatured = 1 AND Status = 'Published' AND IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Videos_Platform",
            table: "Videos",
            column: "Platform");

        migrationBuilder.CreateIndex(
            name: "IX_Videos_PublishedDate",
            table: "Videos",
            column: "PublishedDate",
            filter: "Status = 'Published' AND IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Videos_RelatedRaceId",
            table: "Videos",
            column: "RelatedRaceId");

        migrationBuilder.CreateIndex(
            name: "IX_Videos_RelatedRiderId",
            table: "Videos",
            column: "RelatedRiderId");

        migrationBuilder.CreateIndex(
            name: "IX_Videos_RelatedSeasonId",
            table: "Videos",
            column: "RelatedSeasonId");

        migrationBuilder.CreateIndex(
            name: "IX_Videos_RelatedTeamId",
            table: "Videos",
            column: "RelatedTeamId");

        migrationBuilder.CreateIndex(
            name: "IX_Videos_Status",
            table: "Videos",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "IX_Videos_Type",
            table: "Videos",
            column: "Type");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ActionInFunctions");

        migrationBuilder.DropTable(
            name: "AspNetRoleClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserLogins");

        migrationBuilder.DropTable(
            name: "AspNetUserRoles");

        migrationBuilder.DropTable(
            name: "AspNetUserTokens");

        migrationBuilder.DropTable(
            name: "Bikes");

        migrationBuilder.DropTable(
            name: "News");

        migrationBuilder.DropTable(
            name: "Permissions");

        migrationBuilder.DropTable(
            name: "Product");

        migrationBuilder.DropTable(
            name: "RaceEntries");

        migrationBuilder.DropTable(
            name: "RiderTeamHistories");

        migrationBuilder.DropTable(
            name: "Videos");

        migrationBuilder.DropTable(
            name: "AspNetUsers");

        migrationBuilder.DropTable(
            name: "Actions");

        migrationBuilder.DropTable(
            name: "AspNetRoles");

        migrationBuilder.DropTable(
            name: "Functions");

        migrationBuilder.DropTable(
            name: "Races");

        migrationBuilder.DropTable(
            name: "Riders");

        migrationBuilder.DropTable(
            name: "Seasons");

        migrationBuilder.DropTable(
            name: "Teams");
    }
}
