using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class addbob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a20eb2db-a843-4670-86fe-91c51d6cd08f", "c9d510c5-3e82-4d70-bc69-b310f6fc10d9" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "5e43f8ef-4abd-4e50-be4e-d4cab90c4724", "797a92f8-cf25-4f4e-bbbd-892e073f6daf" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Birthdate", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImageUrl", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SearchOrganism", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "user-3", 0, new DateOnly(1995, 6, 15), "37cdf7c8-d666-4186-bc5a-17ae022e7f79", "bob@biodomes.com", true, null, false, null, "BOB@BIODOMES.COM", "BOB", null, null, false, "Marie Thérèse", "0b0cb2d4-b0b5-4c43-b070-b8ede3ec81cc", false, "bob" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-3");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "dd846e1f-d948-420b-af1f-4c36fb9ba85d", "8937fe83-549e-493f-a4b6-79f1ae4fdb94" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2c59a1a3-9e94-46c6-9824-4da3fef7a162", "ad5877b3-25a8-4dbc-ba2e-4f6ea086f57d" });
        }
    }
}
