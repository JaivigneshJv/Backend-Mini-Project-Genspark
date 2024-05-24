using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleBankingSystemAPI.Migrations
{
    public partial class AddEmailVerification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("78799cf1-2829-48b9-aeb7-b2de3c9c2708"));

            migrationBuilder.CreateTable(
                name: "EmailVerifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVerifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("9500dcf2-c726-4eca-8bc9-c6e52c4b7c82"), "1234567890", new DateTime(2024, 5, 24, 6, 13, 4, 440, DateTimeKind.Utc).AddTicks(1344), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 133, 167, 150, 51, 65, 57, 150, 104, 153, 126, 97, 126, 223, 97, 250, 25, 9, 106, 81, 179, 219, 21, 181, 241, 216, 161, 172, 63, 52, 246, 77, 96, 141, 46, 84, 248, 246, 169, 38, 119, 155, 196, 168, 170, 154, 97, 230, 178, 120, 174, 166, 7, 130, 159, 213, 0, 81, 119, 172, 38, 20, 126, 120, 57, 157, 31, 159, 15, 185, 0, 242, 194, 121, 5, 168, 98, 112, 202, 247, 68, 143, 50, 180, 45, 201, 48, 7, 98, 112, 159, 44, 173, 111, 66, 30, 170, 79, 178, 241, 135, 27, 205, 51, 129, 27, 121, 147, 38, 134, 42, 91, 221, 189, 231, 26, 169, 10, 32, 152, 96, 95, 245, 134, 142, 188, 168, 16, 197 }, new byte[] { 181, 59, 120, 7, 6, 62, 145, 91, 103, 188, 250, 198, 77, 169, 233, 227, 137, 147, 165, 191, 86, 184, 20, 81, 48, 241, 110, 97, 147, 15, 85, 237, 160, 242, 142, 192, 46, 137, 130, 118, 86, 132, 150, 5, 108, 224, 30, 102, 178, 11, 49, 134, 100, 176, 21, 20, 178, 144, 145, 205, 53, 48, 218, 178 }, "Admin", new DateTime(2024, 5, 24, 6, 13, 4, 440, DateTimeKind.Utc).AddTicks(1345), "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerifications_UserId",
                table: "EmailVerifications",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailVerifications");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9500dcf2-c726-4eca-8bc9-c6e52c4b7c82"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("78799cf1-2829-48b9-aeb7-b2de3c9c2708"), "1234567890", new DateTime(2024, 5, 22, 11, 51, 23, 604, DateTimeKind.Utc).AddTicks(6304), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 239, 239, 106, 133, 212, 62, 252, 229, 161, 201, 213, 60, 185, 155, 135, 68, 124, 245, 140, 224, 61, 197, 170, 22, 2, 47, 7, 190, 250, 131, 238, 127, 17, 94, 216, 58, 248, 46, 214, 242, 97, 171, 191, 44, 67, 160, 119, 113, 207, 161, 8, 48, 63, 151, 129, 40, 124, 83, 250, 16, 165, 209, 217, 115, 246, 242, 128, 157, 133, 112, 14, 71, 38, 154, 133, 207, 133, 134, 209, 115, 215, 239, 196, 170, 189, 8, 68, 8, 210, 12, 221, 209, 253, 173, 225, 155, 7, 254, 225, 100, 195, 240, 102, 50, 245, 157, 247, 58, 63, 109, 244, 42, 84, 75, 3, 85, 215, 235, 37, 155, 76, 192, 141, 233, 127, 128, 71, 99 }, new byte[] { 192, 173, 198, 8, 252, 253, 25, 244, 235, 75, 143, 190, 50, 72, 98, 135, 160, 5, 151, 10, 84, 127, 168, 132, 188, 205, 97, 250, 195, 127, 194, 107, 225, 27, 115, 191, 38, 72, 202, 223, 234, 107, 43, 232, 2, 14, 5, 78, 0, 78, 159, 56, 188, 18, 166, 24, 222, 63, 216, 139, 89, 51, 39, 15 }, "Admin", new DateTime(2024, 5, 22, 11, 51, 23, 604, DateTimeKind.Utc).AddTicks(6305), "Admin" });
        }
    }
}
