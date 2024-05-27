using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleBankingSystemAPI.Migrations
{
    public partial class AddedFieldsAccountsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("63e93a0c-6018-47e7-a205-9e5b3fa647e1"));

            migrationBuilder.AddColumn<byte[]>(
                name: "TransactionPasswordHash",
                table: "Accounts",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TransactionPasswordKey",
                table: "Accounts",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("18c7c770-d7dc-4a27-a3a7-dbb755d9043f"), "1234567890", new DateTime(2024, 5, 24, 12, 0, 28, 293, DateTimeKind.Utc).AddTicks(8261), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 83, 237, 81, 203, 134, 18, 129, 200, 121, 124, 121, 125, 167, 61, 114, 159, 60, 202, 62, 150, 21, 144, 170, 218, 3, 94, 134, 15, 93, 47, 103, 157, 158, 125, 68, 97, 49, 252, 108, 126, 248, 154, 204, 226, 193, 64, 131, 225, 255, 17, 97, 89, 5, 180, 116, 122, 199, 228, 105, 20, 206, 130, 203, 255, 206, 40, 71, 124, 78, 53, 92, 136, 85, 114, 224, 204, 141, 242, 53, 152, 33, 18, 251, 164, 98, 71, 47, 52, 163, 6, 117, 99, 249, 83, 117, 187, 150, 195, 208, 55, 160, 130, 13, 59, 254, 136, 250, 217, 150, 18, 78, 247, 237, 131, 139, 95, 167, 147, 158, 104, 215, 193, 1, 49, 32, 3, 127, 61 }, new byte[] { 142, 112, 101, 72, 132, 248, 124, 185, 81, 205, 29, 173, 17, 137, 76, 234, 83, 233, 255, 6, 52, 125, 32, 159, 132, 52, 147, 94, 173, 224, 175, 165, 128, 235, 173, 180, 95, 116, 26, 140, 150, 179, 153, 241, 189, 14, 229, 192, 168, 93, 51, 38, 80, 178, 62, 142, 154, 135, 115, 150, 108, 67, 64, 111 }, "Admin", new DateTime(2024, 5, 24, 12, 0, 28, 293, DateTimeKind.Utc).AddTicks(8263), "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("18c7c770-d7dc-4a27-a3a7-dbb755d9043f"));

            migrationBuilder.DropColumn(
                name: "TransactionPasswordHash",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "TransactionPasswordKey",
                table: "Accounts");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("63e93a0c-6018-47e7-a205-9e5b3fa647e1"), "1234567890", new DateTime(2024, 5, 24, 7, 34, 16, 597, DateTimeKind.Utc).AddTicks(1321), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 31, 53, 184, 26, 55, 78, 127, 190, 249, 120, 191, 241, 0, 209, 154, 5, 225, 56, 245, 68, 148, 248, 78, 56, 22, 194, 0, 3, 204, 4, 102, 227, 249, 70, 239, 187, 138, 154, 214, 40, 50, 94, 42, 89, 51, 82, 223, 170, 224, 2, 230, 192, 46, 234, 184, 21, 80, 11, 115, 139, 66, 69, 31, 228, 204, 230, 73, 214, 117, 18, 245, 95, 168, 153, 221, 217, 91, 138, 113, 57, 81, 89, 60, 227, 18, 159, 100, 145, 245, 115, 154, 16, 184, 105, 39, 155, 194, 134, 8, 169, 153, 224, 138, 183, 98, 125, 68, 114, 84, 216, 48, 221, 109, 89, 193, 29, 220, 170, 163, 139, 185, 96, 204, 184, 174, 160, 94, 155 }, new byte[] { 134, 249, 67, 223, 106, 233, 237, 237, 22, 139, 96, 8, 115, 117, 97, 163, 238, 235, 86, 10, 53, 169, 153, 243, 196, 130, 74, 157, 23, 64, 29, 20, 47, 97, 98, 125, 152, 102, 181, 182, 209, 42, 193, 40, 222, 241, 43, 69, 19, 147, 66, 149, 155, 193, 146, 235, 130, 204, 162, 7, 8, 252, 70, 231 }, "Admin", new DateTime(2024, 5, 24, 7, 34, 16, 597, DateTimeKind.Utc).AddTicks(1323), "Admin" });
        }
    }
}
