using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleBankingSystemAPI.Migrations
{
    public partial class TransactionVerificationTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("857c0184-4995-4d5d-b3e8-f4720dca1e19"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("f140311d-ae4f-4d73-b7f8-31e55ebd58de"), "1234567890", new DateTime(2024, 5, 24, 15, 32, 26, 557, DateTimeKind.Utc).AddTicks(4349), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 113, 201, 185, 84, 142, 236, 63, 190, 178, 163, 29, 70, 192, 34, 19, 255, 142, 202, 234, 148, 29, 134, 241, 16, 157, 34, 34, 86, 37, 32, 158, 159, 203, 221, 114, 128, 175, 24, 199, 203, 186, 180, 235, 77, 177, 213, 46, 162, 178, 196, 23, 253, 84, 3, 91, 189, 7, 34, 117, 110, 185, 18, 15, 92, 62, 214, 142, 227, 9, 242, 132, 96, 7, 213, 130, 232, 11, 184, 250, 205, 154, 65, 82, 13, 207, 24, 86, 214, 119, 164, 138, 95, 194, 39, 59, 178, 149, 100, 219, 47, 89, 16, 106, 197, 183, 25, 180, 245, 32, 66, 179, 158, 164, 30, 57, 148, 217, 52, 177, 139, 85, 131, 245, 72, 96, 224, 240, 118 }, new byte[] { 112, 180, 67, 77, 34, 235, 165, 131, 82, 84, 176, 96, 48, 192, 33, 168, 215, 199, 135, 125, 63, 94, 173, 163, 192, 168, 213, 89, 248, 179, 114, 139, 30, 35, 125, 33, 237, 183, 164, 203, 178, 51, 82, 68, 100, 224, 111, 227, 55, 96, 253, 23, 16, 182, 179, 78, 145, 222, 65, 75, 183, 30, 177, 125 }, "Admin", new DateTime(2024, 5, 24, 15, 32, 26, 557, DateTimeKind.Utc).AddTicks(4351), "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f140311d-ae4f-4d73-b7f8-31e55ebd58de"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("857c0184-4995-4d5d-b3e8-f4720dca1e19"), "1234567890", new DateTime(2024, 5, 24, 14, 15, 53, 421, DateTimeKind.Utc).AddTicks(2065), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 12, 241, 124, 228, 38, 168, 34, 233, 70, 49, 206, 113, 219, 160, 118, 79, 176, 211, 23, 163, 100, 16, 227, 86, 123, 102, 157, 204, 91, 113, 12, 25, 175, 176, 206, 125, 50, 245, 245, 111, 73, 255, 199, 19, 67, 124, 177, 216, 146, 139, 24, 83, 204, 161, 235, 251, 189, 192, 190, 68, 101, 127, 111, 225, 48, 147, 166, 181, 71, 50, 138, 243, 94, 185, 121, 4, 142, 219, 114, 154, 125, 66, 86, 126, 76, 216, 218, 35, 243, 237, 178, 110, 25, 47, 217, 44, 136, 66, 84, 1, 198, 199, 234, 250, 116, 6, 252, 83, 188, 26, 133, 48, 66, 160, 255, 219, 60, 131, 198, 39, 123, 55, 55, 215, 53, 7, 252, 133 }, new byte[] { 102, 87, 137, 142, 57, 58, 171, 205, 187, 11, 231, 53, 218, 245, 185, 13, 197, 224, 251, 252, 119, 18, 22, 170, 51, 160, 30, 32, 92, 110, 207, 133, 107, 88, 41, 218, 249, 115, 34, 82, 102, 241, 88, 42, 163, 129, 181, 186, 250, 243, 81, 207, 85, 204, 125, 103, 171, 124, 227, 182, 160, 164, 9, 147 }, "Admin", new DateTime(2024, 5, 24, 14, 15, 53, 421, DateTimeKind.Utc).AddTicks(2066), "Admin" });
        }
    }
}
