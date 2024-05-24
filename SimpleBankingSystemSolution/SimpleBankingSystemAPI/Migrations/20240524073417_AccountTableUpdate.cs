using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleBankingSystemAPI.Migrations
{
    public partial class AccountTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9500dcf2-c726-4eca-8bc9-c6e52c4b7c82"));

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PendingUserProfileUpdates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsRejected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingUserProfileUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingUserProfileUpdates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("63e93a0c-6018-47e7-a205-9e5b3fa647e1"), "1234567890", new DateTime(2024, 5, 24, 7, 34, 16, 597, DateTimeKind.Utc).AddTicks(1321), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 31, 53, 184, 26, 55, 78, 127, 190, 249, 120, 191, 241, 0, 209, 154, 5, 225, 56, 245, 68, 148, 248, 78, 56, 22, 194, 0, 3, 204, 4, 102, 227, 249, 70, 239, 187, 138, 154, 214, 40, 50, 94, 42, 89, 51, 82, 223, 170, 224, 2, 230, 192, 46, 234, 184, 21, 80, 11, 115, 139, 66, 69, 31, 228, 204, 230, 73, 214, 117, 18, 245, 95, 168, 153, 221, 217, 91, 138, 113, 57, 81, 89, 60, 227, 18, 159, 100, 145, 245, 115, 154, 16, 184, 105, 39, 155, 194, 134, 8, 169, 153, 224, 138, 183, 98, 125, 68, 114, 84, 216, 48, 221, 109, 89, 193, 29, 220, 170, 163, 139, 185, 96, 204, 184, 174, 160, 94, 155 }, new byte[] { 134, 249, 67, 223, 106, 233, 237, 237, 22, 139, 96, 8, 115, 117, 97, 163, 238, 235, 86, 10, 53, 169, 153, 243, 196, 130, 74, 157, 23, 64, 29, 20, 47, 97, 98, 125, 152, 102, 181, 182, 209, 42, 193, 40, 222, 241, 43, 69, 19, 147, 66, 149, 155, 193, 146, 235, 130, 204, 162, 7, 8, 252, 70, 231 }, "Admin", new DateTime(2024, 5, 24, 7, 34, 16, 597, DateTimeKind.Utc).AddTicks(1323), "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_PendingUserProfileUpdates_UserId",
                table: "PendingUserProfileUpdates",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingUserProfileUpdates");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("63e93a0c-6018-47e7-a205-9e5b3fa647e1"));

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Accounts");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("9500dcf2-c726-4eca-8bc9-c6e52c4b7c82"), "1234567890", new DateTime(2024, 5, 24, 6, 13, 4, 440, DateTimeKind.Utc).AddTicks(1344), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 133, 167, 150, 51, 65, 57, 150, 104, 153, 126, 97, 126, 223, 97, 250, 25, 9, 106, 81, 179, 219, 21, 181, 241, 216, 161, 172, 63, 52, 246, 77, 96, 141, 46, 84, 248, 246, 169, 38, 119, 155, 196, 168, 170, 154, 97, 230, 178, 120, 174, 166, 7, 130, 159, 213, 0, 81, 119, 172, 38, 20, 126, 120, 57, 157, 31, 159, 15, 185, 0, 242, 194, 121, 5, 168, 98, 112, 202, 247, 68, 143, 50, 180, 45, 201, 48, 7, 98, 112, 159, 44, 173, 111, 66, 30, 170, 79, 178, 241, 135, 27, 205, 51, 129, 27, 121, 147, 38, 134, 42, 91, 221, 189, 231, 26, 169, 10, 32, 152, 96, 95, 245, 134, 142, 188, 168, 16, 197 }, new byte[] { 181, 59, 120, 7, 6, 62, 145, 91, 103, 188, 250, 198, 77, 169, 233, 227, 137, 147, 165, 191, 86, 184, 20, 81, 48, 241, 110, 97, 147, 15, 85, 237, 160, 242, 142, 192, 46, 137, 130, 118, 86, 132, 150, 5, 108, 224, 30, 102, 178, 11, 49, 134, 100, 176, 21, 20, 178, 144, 145, 205, 53, 48, 218, 178 }, "Admin", new DateTime(2024, 5, 24, 6, 13, 4, 440, DateTimeKind.Utc).AddTicks(1345), "Admin" });
        }
    }
}
