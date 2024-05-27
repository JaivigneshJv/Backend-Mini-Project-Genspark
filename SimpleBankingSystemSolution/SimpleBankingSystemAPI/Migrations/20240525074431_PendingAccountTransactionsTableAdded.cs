using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleBankingSystemAPI.Migrations
{
    public partial class PendingAccountTransactionsTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5c0b480f-a236-4991-b7cf-cce57f99056b"));

            migrationBuilder.CreateTable(
                name: "PendingAccountTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsRejected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingAccountTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingAccountTransactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("1768a861-c2ca-446a-a020-428b7eb481b5"), "1234567890", new DateTime(2024, 5, 25, 7, 44, 31, 222, DateTimeKind.Utc).AddTicks(4366), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 132, 199, 28, 180, 124, 10, 1, 42, 131, 97, 81, 118, 234, 172, 75, 18, 152, 114, 108, 169, 104, 32, 1, 39, 241, 71, 221, 93, 136, 20, 31, 205, 223, 32, 132, 1, 101, 215, 18, 20, 25, 111, 101, 232, 173, 4, 54, 112, 118, 163, 39, 24, 245, 77, 181, 131, 166, 22, 134, 172, 114, 237, 48, 189, 178, 180, 240, 212, 91, 102, 76, 94, 249, 36, 230, 14, 229, 206, 68, 35, 58, 104, 31, 159, 18, 58, 137, 31, 194, 40, 110, 138, 209, 245, 37, 230, 151, 26, 197, 95, 31, 46, 179, 228, 98, 35, 132, 110, 35, 190, 229, 88, 23, 169, 74, 180, 242, 193, 134, 157, 20, 220, 189, 239, 5, 195, 225, 206 }, new byte[] { 188, 142, 55, 216, 140, 3, 222, 66, 50, 195, 59, 34, 4, 149, 251, 144, 100, 187, 220, 201, 87, 196, 101, 137, 149, 212, 152, 59, 27, 47, 110, 232, 140, 20, 42, 215, 81, 226, 181, 5, 254, 241, 130, 16, 55, 251, 24, 10, 134, 165, 136, 226, 37, 227, 50, 28, 242, 82, 101, 82, 237, 105, 137, 183 }, "Admin", new DateTime(2024, 5, 25, 7, 44, 31, 222, DateTimeKind.Utc).AddTicks(4368), "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_PendingAccountTransactions_AccountId",
                table: "PendingAccountTransactions",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingAccountTransactions");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1768a861-c2ca-446a-a020-428b7eb481b5"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("5c0b480f-a236-4991-b7cf-cce57f99056b"), "1234567890", new DateTime(2024, 5, 25, 5, 4, 11, 49, DateTimeKind.Utc).AddTicks(4381), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 28, 106, 135, 200, 179, 99, 146, 224, 70, 55, 77, 142, 120, 140, 91, 56, 118, 6, 164, 42, 220, 103, 124, 83, 83, 248, 190, 27, 109, 242, 153, 210, 104, 171, 220, 34, 247, 140, 209, 177, 232, 162, 29, 148, 224, 157, 179, 163, 230, 246, 85, 3, 99, 101, 104, 120, 176, 19, 38, 164, 223, 76, 5, 57, 27, 153, 241, 243, 28, 250, 4, 130, 49, 184, 70, 33, 199, 120, 201, 136, 92, 163, 65, 97, 34, 111, 132, 193, 170, 75, 29, 222, 0, 183, 184, 4, 53, 228, 107, 167, 47, 197, 14, 32, 119, 40, 93, 81, 103, 119, 56, 153, 2, 56, 201, 231, 128, 102, 50, 4, 66, 205, 12, 95, 4, 119, 199, 5 }, new byte[] { 170, 159, 178, 220, 205, 80, 51, 136, 16, 152, 242, 240, 225, 116, 50, 163, 163, 36, 124, 142, 13, 174, 229, 162, 158, 106, 195, 93, 45, 180, 137, 116, 46, 95, 59, 124, 190, 240, 195, 190, 143, 124, 164, 189, 25, 76, 71, 170, 48, 116, 150, 169, 23, 39, 217, 47, 180, 212, 105, 6, 136, 212, 179, 229 }, "Admin", new DateTime(2024, 5, 25, 5, 4, 11, 49, DateTimeKind.Utc).AddTicks(4382), "Admin" });
        }
    }
}
