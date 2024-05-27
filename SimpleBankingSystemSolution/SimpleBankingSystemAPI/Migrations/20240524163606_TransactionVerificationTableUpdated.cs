using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleBankingSystemAPI.Migrations
{
    public partial class TransactionVerificationTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f140311d-ae4f-4d73-b7f8-31e55ebd58de"));

            migrationBuilder.CreateTable(
                name: "TransactionVerifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionVerifications_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("321cbd77-0a66-488a-b27a-7639ffa99571"), "1234567890", new DateTime(2024, 5, 24, 16, 36, 6, 237, DateTimeKind.Utc).AddTicks(8989), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 236, 108, 12, 172, 17, 73, 192, 119, 147, 196, 89, 231, 50, 188, 122, 252, 91, 250, 160, 153, 101, 61, 122, 177, 68, 171, 78, 174, 129, 47, 79, 158, 128, 60, 157, 23, 101, 154, 158, 197, 73, 101, 209, 185, 43, 19, 187, 99, 160, 191, 149, 64, 96, 83, 66, 67, 250, 191, 175, 79, 113, 226, 160, 254, 213, 198, 141, 94, 76, 54, 31, 199, 27, 146, 197, 154, 114, 204, 116, 179, 139, 121, 52, 237, 15, 166, 228, 97, 74, 146, 176, 85, 149, 54, 189, 131, 91, 68, 103, 118, 189, 178, 33, 26, 125, 179, 55, 18, 85, 201, 135, 60, 129, 175, 234, 114, 55, 150, 251, 3, 112, 112, 7, 173, 168, 85, 173, 200 }, new byte[] { 25, 167, 183, 161, 69, 48, 135, 236, 234, 80, 12, 87, 172, 134, 75, 92, 188, 4, 92, 208, 51, 221, 103, 156, 132, 232, 61, 251, 76, 79, 144, 106, 1, 155, 59, 235, 237, 8, 89, 250, 43, 77, 244, 40, 195, 12, 162, 219, 213, 45, 81, 53, 97, 175, 69, 149, 118, 183, 200, 14, 206, 220, 159, 150 }, "Admin", new DateTime(2024, 5, 24, 16, 36, 6, 237, DateTimeKind.Utc).AddTicks(8990), "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionVerifications_AccountId",
                table: "TransactionVerifications",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionVerifications");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("321cbd77-0a66-488a-b27a-7639ffa99571"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Contact", "CreatedDate", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "Role", "UpdatedDate", "Username" },
                values: new object[] { new Guid("f140311d-ae4f-4d73-b7f8-31e55ebd58de"), "1234567890", new DateTime(2024, 5, 24, 15, 32, 26, 557, DateTimeKind.Utc).AddTicks(4349), new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminuser@simplebank.com", "Admin", true, "User", new byte[] { 113, 201, 185, 84, 142, 236, 63, 190, 178, 163, 29, 70, 192, 34, 19, 255, 142, 202, 234, 148, 29, 134, 241, 16, 157, 34, 34, 86, 37, 32, 158, 159, 203, 221, 114, 128, 175, 24, 199, 203, 186, 180, 235, 77, 177, 213, 46, 162, 178, 196, 23, 253, 84, 3, 91, 189, 7, 34, 117, 110, 185, 18, 15, 92, 62, 214, 142, 227, 9, 242, 132, 96, 7, 213, 130, 232, 11, 184, 250, 205, 154, 65, 82, 13, 207, 24, 86, 214, 119, 164, 138, 95, 194, 39, 59, 178, 149, 100, 219, 47, 89, 16, 106, 197, 183, 25, 180, 245, 32, 66, 179, 158, 164, 30, 57, 148, 217, 52, 177, 139, 85, 131, 245, 72, 96, 224, 240, 118 }, new byte[] { 112, 180, 67, 77, 34, 235, 165, 131, 82, 84, 176, 96, 48, 192, 33, 168, 215, 199, 135, 125, 63, 94, 173, 163, 192, 168, 213, 89, 248, 179, 114, 139, 30, 35, 125, 33, 237, 183, 164, 203, 178, 51, 82, 68, 100, 224, 111, 227, 55, 96, 253, 23, 16, 182, 179, 78, 145, 222, 65, 75, 183, 30, 177, 125 }, "Admin", new DateTime(2024, 5, 24, 15, 32, 26, 557, DateTimeKind.Utc).AddTicks(4351), "Admin" });
        }
    }
}
