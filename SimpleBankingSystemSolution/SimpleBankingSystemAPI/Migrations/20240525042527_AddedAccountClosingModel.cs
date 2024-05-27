using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleBankingSystemAPI.Migrations
{
    public partial class AddedAccountClosingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("321cbd77-0a66-488a-b27a-7639ffa99571"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("1499dad1-4e38-436f-80e1-f094c2098065"), "1234567890", new DateTime(2024, 5, 25, 4, 25, 27, 98, DateTimeKind.Utc).AddTicks(182), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 70, 80, 74, 125, 103, 128, 148, 217, 36, 213, 51, 218, 123, 80, 28, 173, 173, 170, 145, 198, 230, 184, 6, 8, 7, 93, 100, 190, 149, 157, 111, 39, 217, 19, 41, 25, 165, 137, 207, 53, 197, 34, 137, 102, 182, 237, 96, 75, 92, 119, 160, 152, 75, 197, 73, 51, 34, 20, 60, 140, 59, 235, 133, 156, 64, 35, 119, 232, 93, 146, 54, 37, 3, 3, 96, 237, 28, 40, 68, 168, 126, 213, 16, 190, 64, 107, 252, 186, 132, 124, 17, 12, 251, 21, 54, 178, 236, 230, 157, 38, 2, 15, 212, 211, 227, 2, 118, 161, 180, 161, 132, 176, 63, 109, 239, 125, 209, 56, 206, 184, 164, 59, 33, 233, 43, 59, 193, 170 }, new byte[] { 147, 221, 5, 164, 17, 250, 88, 241, 60, 100, 226, 138, 75, 33, 8, 27, 197, 47, 114, 239, 170, 45, 159, 135, 206, 7, 210, 187, 81, 3, 161, 165, 23, 153, 5, 0, 231, 22, 149, 107, 243, 233, 218, 105, 28, 88, 18, 4, 121, 44, 154, 253, 57, 40, 206, 25, 15, 139, 55, 1, 151, 88, 173, 140 }, "Admin", new DateTime(2024, 5, 25, 4, 25, 27, 98, DateTimeKind.Utc).AddTicks(184), "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1499dad1-4e38-436f-80e1-f094c2098065"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("321cbd77-0a66-488a-b27a-7639ffa99571"), "1234567890", new DateTime(2024, 5, 24, 16, 36, 6, 237, DateTimeKind.Utc).AddTicks(8989), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 236, 108, 12, 172, 17, 73, 192, 119, 147, 196, 89, 231, 50, 188, 122, 252, 91, 250, 160, 153, 101, 61, 122, 177, 68, 171, 78, 174, 129, 47, 79, 158, 128, 60, 157, 23, 101, 154, 158, 197, 73, 101, 209, 185, 43, 19, 187, 99, 160, 191, 149, 64, 96, 83, 66, 67, 250, 191, 175, 79, 113, 226, 160, 254, 213, 198, 141, 94, 76, 54, 31, 199, 27, 146, 197, 154, 114, 204, 116, 179, 139, 121, 52, 237, 15, 166, 228, 97, 74, 146, 176, 85, 149, 54, 189, 131, 91, 68, 103, 118, 189, 178, 33, 26, 125, 179, 55, 18, 85, 201, 135, 60, 129, 175, 234, 114, 55, 150, 251, 3, 112, 112, 7, 173, 168, 85, 173, 200 }, new byte[] { 25, 167, 183, 161, 69, 48, 135, 236, 234, 80, 12, 87, 172, 134, 75, 92, 188, 4, 92, 208, 51, 221, 103, 156, 132, 232, 61, 251, 76, 79, 144, 106, 1, 155, 59, 235, 237, 8, 89, 250, 43, 77, 244, 40, 195, 12, 162, 219, 213, 45, 81, 53, 97, 175, 69, 149, 118, 183, 200, 14, 206, 220, 159, 150 }, "Admin", new DateTime(2024, 5, 24, 16, 36, 6, 237, DateTimeKind.Utc).AddTicks(8990), "Admin" });
        }
    }
}
