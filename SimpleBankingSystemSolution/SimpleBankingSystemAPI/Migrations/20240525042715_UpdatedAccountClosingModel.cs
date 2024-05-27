using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleBankingSystemAPI.Migrations
{
    public partial class UpdatedAccountClosingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1499dad1-4e38-436f-80e1-f094c2098065"));

            migrationBuilder.CreateTable(
                name: "PendingAccountClosing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingAccountClosing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingAccountClosing_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("299b503d-60fb-4dd1-a888-3d3e23e9de56"), "1234567890", new DateTime(2024, 5, 25, 4, 27, 15, 191, DateTimeKind.Utc).AddTicks(1595), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 23, 21, 235, 13, 86, 100, 217, 35, 30, 224, 211, 57, 52, 0, 56, 200, 239, 117, 12, 181, 100, 114, 223, 166, 137, 144, 148, 147, 242, 3, 34, 224, 21, 15, 128, 146, 87, 25, 128, 170, 170, 117, 231, 235, 111, 93, 241, 222, 198, 109, 176, 51, 59, 62, 93, 206, 163, 199, 142, 64, 242, 159, 37, 207, 181, 228, 111, 83, 122, 20, 141, 255, 226, 183, 210, 172, 116, 194, 84, 85, 99, 143, 158, 10, 145, 220, 4, 80, 9, 226, 37, 225, 111, 123, 4, 138, 85, 148, 251, 2, 118, 214, 216, 74, 184, 79, 48, 177, 240, 60, 224, 130, 247, 104, 243, 102, 16, 213, 117, 230, 135, 223, 96, 85, 91, 53, 108, 102 }, new byte[] { 127, 162, 18, 209, 73, 166, 238, 186, 21, 217, 43, 24, 5, 238, 116, 87, 31, 71, 6, 220, 154, 179, 172, 74, 209, 128, 195, 228, 179, 49, 39, 194, 8, 131, 20, 102, 105, 225, 68, 133, 179, 61, 4, 3, 245, 244, 120, 121, 236, 55, 93, 45, 240, 79, 1, 7, 227, 248, 23, 38, 231, 164, 39, 85 }, "Admin", new DateTime(2024, 5, 25, 4, 27, 15, 191, DateTimeKind.Utc).AddTicks(1596), "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_PendingAccountClosing_AccountId",
                table: "PendingAccountClosing",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingAccountClosing");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("299b503d-60fb-4dd1-a888-3d3e23e9de56"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("1499dad1-4e38-436f-80e1-f094c2098065"), "1234567890", new DateTime(2024, 5, 25, 4, 25, 27, 98, DateTimeKind.Utc).AddTicks(182), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 70, 80, 74, 125, 103, 128, 148, 217, 36, 213, 51, 218, 123, 80, 28, 173, 173, 170, 145, 198, 230, 184, 6, 8, 7, 93, 100, 190, 149, 157, 111, 39, 217, 19, 41, 25, 165, 137, 207, 53, 197, 34, 137, 102, 182, 237, 96, 75, 92, 119, 160, 152, 75, 197, 73, 51, 34, 20, 60, 140, 59, 235, 133, 156, 64, 35, 119, 232, 93, 146, 54, 37, 3, 3, 96, 237, 28, 40, 68, 168, 126, 213, 16, 190, 64, 107, 252, 186, 132, 124, 17, 12, 251, 21, 54, 178, 236, 230, 157, 38, 2, 15, 212, 211, 227, 2, 118, 161, 180, 161, 132, 176, 63, 109, 239, 125, 209, 56, 206, 184, 164, 59, 33, 233, 43, 59, 193, 170 }, new byte[] { 147, 221, 5, 164, 17, 250, 88, 241, 60, 100, 226, 138, 75, 33, 8, 27, 197, 47, 114, 239, 170, 45, 159, 135, 206, 7, 210, 187, 81, 3, 161, 165, 23, 153, 5, 0, 231, 22, 149, 107, 243, 233, 218, 105, 28, 88, 18, 4, 121, 44, 154, 253, 57, 40, 206, 25, 15, 139, 55, 1, 151, 88, 173, 140 }, "Admin", new DateTime(2024, 5, 25, 4, 25, 27, 98, DateTimeKind.Utc).AddTicks(184), "Admin" });
        }
    }
}
