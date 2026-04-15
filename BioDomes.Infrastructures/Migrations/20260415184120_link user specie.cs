using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class linkuserspecie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "species",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "969facba-a004-4698-858d-905355fc0be9", "9384203e-4f23-4808-895f-cdbe3a521b6d" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "159d6cc7-0409-4e14-bca1-f4260e7bd2c3", "598d3034-207b-46cb-a0b8-0eeff0567a1d" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "araignee",
                columns: new[] { "IsPublic", "UserId" },
                values: new object[] { true, "user-2" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "boa",
                columns: new[] { "IsPublic", "UserId" },
                values: new object[] { true, "user-2" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "chene",
                columns: new[] { "IsPublic", "UserId" },
                values: new object[] { true, "user-2" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "hetre",
                column: "UserId",
                value: "user-1");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "hirondelle",
                column: "UserId",
                value: "user-1");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "lievre",
                columns: new[] { "IsPublic", "UserId" },
                values: new object[] { true, "user-2" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "lion",
                column: "UserId",
                value: "user-1");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "mouche",
                column: "UserId",
                value: "user-1");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "paturin-des-pres",
                column: "UserId",
                value: "user-1");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "souris",
                columns: new[] { "IsPublic", "UserId" },
                values: new object[] { true, "user-2" });

            migrationBuilder.CreateIndex(
                name: "IX_species_UserId",
                schema: "dbo",
                table: "species",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_species_AspNetUsers_UserId",
                schema: "dbo",
                table: "species",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_species_AspNetUsers_UserId",
                schema: "dbo",
                table: "species");

            migrationBuilder.DropIndex(
                name: "IX_species_UserId",
                schema: "dbo",
                table: "species");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "species");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6325675d-c665-4e97-9c94-d00854804b5b", "3c24aeb2-242c-4307-8a0b-87928f7829cf" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "79c915a6-3c7c-4833-a969-d784b4e248e8", "48d8f95d-b5ec-42fc-aab4-a90154ca60ac" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "araignee",
                column: "IsPublic",
                value: false);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "boa",
                column: "IsPublic",
                value: false);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "chene",
                column: "IsPublic",
                value: false);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "lievre",
                column: "IsPublic",
                value: false);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "souris",
                column: "IsPublic",
                value: false);
        }
    }
}
