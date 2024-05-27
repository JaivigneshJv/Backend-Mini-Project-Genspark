using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleBankingSystemAPI.Migrations
{
    public partial class UpdateTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionTypes_TransactionTypeId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionTypeId",
                table: "Transactions");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("18c7c770-d7dc-4a27-a3a7-dbb755d9043f"));

            migrationBuilder.DropColumn(
                name: "TransactionTypeId",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "TransactionType",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("857c0184-4995-4d5d-b3e8-f4720dca1e19"), "1234567890", new DateTime(2024, 5, 24, 14, 15, 53, 421, DateTimeKind.Utc).AddTicks(2065), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 12, 241, 124, 228, 38, 168, 34, 233, 70, 49, 206, 113, 219, 160, 118, 79, 176, 211, 23, 163, 100, 16, 227, 86, 123, 102, 157, 204, 91, 113, 12, 25, 175, 176, 206, 125, 50, 245, 245, 111, 73, 255, 199, 19, 67, 124, 177, 216, 146, 139, 24, 83, 204, 161, 235, 251, 189, 192, 190, 68, 101, 127, 111, 225, 48, 147, 166, 181, 71, 50, 138, 243, 94, 185, 121, 4, 142, 219, 114, 154, 125, 66, 86, 126, 76, 216, 218, 35, 243, 237, 178, 110, 25, 47, 217, 44, 136, 66, 84, 1, 198, 199, 234, 250, 116, 6, 252, 83, 188, 26, 133, 48, 66, 160, 255, 219, 60, 131, 198, 39, 123, 55, 55, 215, 53, 7, 252, 133 }, new byte[] { 102, 87, 137, 142, 57, 58, 171, 205, 187, 11, 231, 53, 218, 245, 185, 13, 197, 224, 251, 252, 119, 18, 22, 170, 51, 160, 30, 32, 92, 110, 207, 133, 107, 88, 41, 218, 249, 115, 34, 82, 102, 241, 88, 42, 163, 129, 181, 186, 250, 243, 81, 207, 85, 204, 125, 103, 171, 124, 227, 182, 160, 164, 9, 147 }, "Admin", new DateTime(2024, 5, 24, 14, 15, 53, 421, DateTimeKind.Utc).AddTicks(2066), "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("857c0184-4995-4d5d-b3e8-f4720dca1e19"));

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "Transactions");

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionTypeId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    TransactionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RaiseRequest = table.Column<bool>(type: "bit", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.TransactionTypeId);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("18c7c770-d7dc-4a27-a3a7-dbb755d9043f"), "1234567890", new DateTime(2024, 5, 24, 12, 0, 28, 293, DateTimeKind.Utc).AddTicks(8261), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 83, 237, 81, 203, 134, 18, 129, 200, 121, 124, 121, 125, 167, 61, 114, 159, 60, 202, 62, 150, 21, 144, 170, 218, 3, 94, 134, 15, 93, 47, 103, 157, 158, 125, 68, 97, 49, 252, 108, 126, 248, 154, 204, 226, 193, 64, 131, 225, 255, 17, 97, 89, 5, 180, 116, 122, 199, 228, 105, 20, 206, 130, 203, 255, 206, 40, 71, 124, 78, 53, 92, 136, 85, 114, 224, 204, 141, 242, 53, 152, 33, 18, 251, 164, 98, 71, 47, 52, 163, 6, 117, 99, 249, 83, 117, 187, 150, 195, 208, 55, 160, 130, 13, 59, 254, 136, 250, 217, 150, 18, 78, 247, 237, 131, 139, 95, 167, 147, 158, 104, 215, 193, 1, 49, 32, 3, 127, 61 }, new byte[] { 142, 112, 101, 72, 132, 248, 124, 185, 81, 205, 29, 173, 17, 137, 76, 234, 83, 233, 255, 6, 52, 125, 32, 159, 132, 52, 147, 94, 173, 224, 175, 165, 128, 235, 173, 180, 95, 116, 26, 140, 150, 179, 153, 241, 189, 14, 229, 192, 168, 93, 51, 38, 80, 178, 62, 142, 154, 135, 115, 150, 108, 67, 64, 111 }, "Admin", new DateTime(2024, 5, 24, 12, 0, 28, 293, DateTimeKind.Utc).AddTicks(8263), "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionTypeId",
                table: "Transactions",
                column: "TransactionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionTypes_TransactionTypeId",
                table: "Transactions",
                column: "TransactionTypeId",
                principalTable: "TransactionTypes",
                principalColumn: "TransactionTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
