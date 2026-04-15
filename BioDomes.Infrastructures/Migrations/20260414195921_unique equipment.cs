using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class uniqueequipment : Migration
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
                values: new object[] { "6325675d-c665-4e97-9c94-d00854804b5b", "3c24aeb2-242c-4307-8a0b-87928f7829cf" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "79c915a6-3c7c-4833-a969-d784b4e248e8", "48d8f95d-b5ec-42fc-aab4-a90154ca60ac" });

            migrationBuilder.CreateIndex(
                name: "IX_equipments_Name",
                schema: "dbo",
                table: "equipments",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_equipments_Name",
                schema: "dbo",
                table: "equipments");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6bf0aec1-2435-4330-bc31-57588915c073", "97cdbea4-3f27-487e-9a3b-eccc88a1c727" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "5e7707ef-0cc6-4a07-837c-4920c95ab471", "34c64448-aa75-42a9-92c3-ab9570572bb0" });
        }
    }
}
