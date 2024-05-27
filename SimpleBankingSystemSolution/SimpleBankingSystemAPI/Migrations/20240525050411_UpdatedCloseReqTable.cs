using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleBankingSystemAPI.Migrations
{
    public partial class UpdatedCloseReqTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("299b503d-60fb-4dd1-a888-3d3e23e9de56"));

            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "PendingAccountClosing",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("5c0b480f-a236-4991-b7cf-cce57f99056b"), "1234567890", new DateTime(2024, 5, 25, 5, 4, 11, 49, DateTimeKind.Utc).AddTicks(4381), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 28, 106, 135, 200, 179, 99, 146, 224, 70, 55, 77, 142, 120, 140, 91, 56, 118, 6, 164, 42, 220, 103, 124, 83, 83, 248, 190, 27, 109, 242, 153, 210, 104, 171, 220, 34, 247, 140, 209, 177, 232, 162, 29, 148, 224, 157, 179, 163, 230, 246, 85, 3, 99, 101, 104, 120, 176, 19, 38, 164, 223, 76, 5, 57, 27, 153, 241, 243, 28, 250, 4, 130, 49, 184, 70, 33, 199, 120, 201, 136, 92, 163, 65, 97, 34, 111, 132, 193, 170, 75, 29, 222, 0, 183, 184, 4, 53, 228, 107, 167, 47, 197, 14, 32, 119, 40, 93, 81, 103, 119, 56, 153, 2, 56, 201, 231, 128, 102, 50, 4, 66, 205, 12, 95, 4, 119, 199, 5 }, new byte[] { 170, 159, 178, 220, 205, 80, 51, 136, 16, 152, 242, 240, 225, 116, 50, 163, 163, 36, 124, 142, 13, 174, 229, 162, 158, 106, 195, 93, 45, 180, 137, 116, 46, 95, 59, 124, 190, 240, 195, 190, 143, 124, 164, 189, 25, 76, 71, 170, 48, 116, 150, 169, 23, 39, 217, 47, 180, 212, 105, 6, 136, 212, 179, 229 }, "Admin", new DateTime(2024, 5, 25, 5, 4, 11, 49, DateTimeKind.Utc).AddTicks(4382), "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5c0b480f-a236-4991-b7cf-cce57f99056b"));

            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "PendingAccountClosing");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("299b503d-60fb-4dd1-a888-3d3e23e9de56"), "1234567890", new DateTime(2024, 5, 25, 4, 27, 15, 191, DateTimeKind.Utc).AddTicks(1595), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 23, 21, 235, 13, 86, 100, 217, 35, 30, 224, 211, 57, 52, 0, 56, 200, 239, 117, 12, 181, 100, 114, 223, 166, 137, 144, 148, 147, 242, 3, 34, 224, 21, 15, 128, 146, 87, 25, 128, 170, 170, 117, 231, 235, 111, 93, 241, 222, 198, 109, 176, 51, 59, 62, 93, 206, 163, 199, 142, 64, 242, 159, 37, 207, 181, 228, 111, 83, 122, 20, 141, 255, 226, 183, 210, 172, 116, 194, 84, 85, 99, 143, 158, 10, 145, 220, 4, 80, 9, 226, 37, 225, 111, 123, 4, 138, 85, 148, 251, 2, 118, 214, 216, 74, 184, 79, 48, 177, 240, 60, 224, 130, 247, 104, 243, 102, 16, 213, 117, 230, 135, 223, 96, 85, 91, 53, 108, 102 }, new byte[] { 127, 162, 18, 209, 73, 166, 238, 186, 21, 217, 43, 24, 5, 238, 116, 87, 31, 71, 6, 220, 154, 179, 172, 74, 209, 128, 195, 228, 179, 49, 39, 194, 8, 131, 20, 102, 105, 225, 68, 133, 179, 61, 4, 3, 245, 244, 120, 121, 236, 55, 93, 45, 240, 79, 1, 7, 227, 248, 23, 38, 231, 164, 39, 85 }, "Admin", new DateTime(2024, 5, 25, 4, 27, 15, 191, DateTimeKind.Utc).AddTicks(1596), "Admin" });
        }
    }
}
