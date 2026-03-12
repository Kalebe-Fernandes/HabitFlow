using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "gamification");

            migrationBuilder.EnsureSchema(
                name: "goals");

            migrationBuilder.EnsureSchema(
                name: "habits");

            migrationBuilder.EnsureSchema(
                name: "users");

            migrationBuilder.CreateTable(
                name: "Badges",
                schema: "gamification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rarity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CriteriaType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CriteriaTargetValue = table.Column<int>(type: "int", nullable: false),
                    Criteria_SpecificHabitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Criteria_SpecificCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyTransactions",
                schema: "gamification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    BalanceAfter = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    RelatedEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Goals",
                schema: "goals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TargetValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TargetUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Habits",
                schema: "habits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IconName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ColorHex = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    FrequencyType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DaysOfWeekFrequency = table.Column<int>(type: "int", nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TargetValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TargetUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    CurrentStreak = table.Column<int>(type: "int", nullable: false),
                    LongestStreak = table.Column<int>(type: "int", nullable: false),
                    TotalCompletions = table.Column<int>(type: "int", nullable: false),
                    XPPerCompletion = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreItems",
                schema: "gamification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ItemData = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLevels",
                schema: "gamification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentLevel = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    TotalXP = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    CurrentBalance = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Profile_Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Profile_DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Profile_Timezone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "America/Sao_Paulo"),
                    Profile_Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "pt-BR"),
                    Profile_AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Settings_NotificationsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Settings_DarkMode = table.Column<bool>(type: "bit", nullable: false),
                    Settings_Theme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XPTransactions",
                schema: "gamification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TotalXPAfter = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    RelatedEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LevelAfter = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XPTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoalHabits",
                schema: "goals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HabitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContributionWeight = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalHabits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoalHabits_Goals_GoalId",
                        column: x => x.GoalId,
                        principalSchema: "goals",
                        principalTable: "Goals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HabitCompletions",
                schema: "habits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HabitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CompletedValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MoodLevel = table.Column<int>(type: "int", nullable: true),
                    EnergyLevel = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitCompletions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitCompletions_Habits_HabitId",
                        column: x => x.HabitId,
                        principalSchema: "habits",
                        principalTable: "Habits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInventories",
                schema: "gamification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreItemId = table.Column<int>(type: "int", nullable: false),
                    PurchasedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEquipped = table.Column<bool>(type: "bit", nullable: false),
                    EquippedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInventories_StoreItems_StoreItemId",
                        column: x => x.StoreItemId,
                        principalSchema: "gamification",
                        principalTable: "StoreItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserBadges",
                schema: "gamification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BadgeId = table.Column<int>(type: "int", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBadges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBadges_UserLevels_UserLevelId",
                        column: x => x.UserLevelId,
                        principalSchema: "gamification",
                        principalTable: "UserLevels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Badges_IsActive",
                schema: "gamification",
                table: "Badges",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Badges_Rarity",
                schema: "gamification",
                table: "Badges",
                column: "Rarity");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTransactions_CreatedAt",
                schema: "gamification",
                table: "CurrencyTransactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTransactions_UserId",
                schema: "gamification",
                table: "CurrencyTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalHabits_GoalId",
                schema: "goals",
                table: "GoalHabits",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalHabits_GoalId_HabitId",
                schema: "goals",
                table: "GoalHabits",
                columns: new[] { "GoalId", "HabitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoalHabits_HabitId",
                schema: "goals",
                table: "GoalHabits",
                column: "HabitId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_UserId",
                schema: "goals",
                table: "Goals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_UserId_Status",
                schema: "goals",
                table: "Goals",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_HabitCompletions_CompletionDate",
                schema: "habits",
                table: "HabitCompletions",
                column: "CompletionDate");

            migrationBuilder.CreateIndex(
                name: "IX_HabitCompletions_HabitId",
                schema: "habits",
                table: "HabitCompletions",
                column: "HabitId");

            migrationBuilder.CreateIndex(
                name: "IX_HabitCompletions_HabitId_CompletionDate",
                schema: "habits",
                table: "HabitCompletions",
                columns: new[] { "HabitId", "CompletionDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Habits_UserId",
                schema: "habits",
                table: "Habits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Habits_UserId_Status",
                schema: "habits",
                table: "Habits",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_StoreItems_Category",
                schema: "gamification",
                table: "StoreItems",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItems_IsActive",
                schema: "gamification",
                table: "StoreItems",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItems_SortOrder",
                schema: "gamification",
                table: "StoreItems",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_UserBadges_UserLevelId",
                schema: "gamification",
                table: "UserBadges",
                column: "UserLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInventories_StoreItemId",
                schema: "gamification",
                table: "UserInventories",
                column: "StoreItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInventories_UserId",
                schema: "gamification",
                table: "UserInventories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInventories_UserId_StoreItemId",
                schema: "gamification",
                table: "UserInventories",
                columns: new[] { "UserId", "StoreItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLevels_Id",
                schema: "gamification",
                table: "UserLevels",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "users",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_XPTransactions_CreatedAt",
                schema: "gamification",
                table: "XPTransactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_XPTransactions_UserId",
                schema: "gamification",
                table: "XPTransactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Badges",
                schema: "gamification");

            migrationBuilder.DropTable(
                name: "CurrencyTransactions",
                schema: "gamification");

            migrationBuilder.DropTable(
                name: "GoalHabits",
                schema: "goals");

            migrationBuilder.DropTable(
                name: "HabitCompletions",
                schema: "habits");

            migrationBuilder.DropTable(
                name: "UserBadges",
                schema: "gamification");

            migrationBuilder.DropTable(
                name: "UserInventories",
                schema: "gamification");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "users");

            migrationBuilder.DropTable(
                name: "XPTransactions",
                schema: "gamification");

            migrationBuilder.DropTable(
                name: "Goals",
                schema: "goals");

            migrationBuilder.DropTable(
                name: "Habits",
                schema: "habits");

            migrationBuilder.DropTable(
                name: "UserLevels",
                schema: "gamification");

            migrationBuilder.DropTable(
                name: "StoreItems",
                schema: "gamification");
        }
    }
}
