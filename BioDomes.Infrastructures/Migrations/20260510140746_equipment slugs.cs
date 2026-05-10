using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class equipmentslugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                schema: "dbo",
                table: "equipment_resources",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3f1551d6-ff3f-4754-9f57-2efe8ca0edf9", "3c883524-a9f6-4398-b2fc-e20dd7d4da0f" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "28686c89-e70e-48b7-9ad2-0c3ccdd95e3a", "9aff6ae9-ff34-405b-8d28-899e561e50e6" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-3",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "cf6df051-7d66-4313-a460-e0cae0a3bfe9", "f3674868-b7bb-4cba-82b2-b1be62a682a5" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipment_resources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Slug",
                value: "lumiere");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipment_resources",
                keyColumn: "Id",
                keyValue: 2,
                column: "Slug",
                value: "eau");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipment_resources",
                keyColumn: "Id",
                keyValue: 3,
                column: "Slug",
                value: "azote");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipment_resources",
                keyColumn: "Id",
                keyValue: 4,
                column: "Slug",
                value: "hydrogene");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipment_resources",
                keyColumn: "Id",
                keyValue: 5,
                column: "Slug",
                value: "oxygene");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipment_resources",
                keyColumn: "Id",
                keyValue: 6,
                column: "Slug",
                value: "co2");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipment_resources",
                keyColumn: "Id",
                keyValue: 7,
                column: "Slug",
                value: "phosphore");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipment_resources",
                keyColumn: "Id",
                keyValue: 8,
                column: "Slug",
                value: "potassium");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipment_resources",
                keyColumn: "Id",
                keyValue: 9,
                column: "Slug",
                value: "calcium");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipment_resources",
                keyColumn: "Id",
                keyValue: 10,
                column: "Slug",
                value: "magnesium");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipment_resources",
                keyColumn: "Id",
                keyValue: 11,
                column: "Slug",
                value: "energie");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipment_resources",
                keyColumn: "Id",
                keyValue: 12,
                column: "Slug",
                value: "matiere-organique");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_resources_Slug",
                schema: "dbo",
                table: "equipment_resources",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_equipment_resources_Slug",
                schema: "dbo",
                table: "equipment_resources");

            migrationBuilder.DropColumn(
                name: "Slug",
                schema: "dbo",
                table: "equipment_resources");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8c409f09-5e3c-47d9-9066-d2158e72aa05", "e0466feb-a308-4b78-be46-2d75db6202d5" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8a700a51-9772-4931-a4e1-701021463768", "7d9b6530-79f9-4bd1-b248-4f84529d4963" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-3",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a8e6abc7-eb59-4450-a29a-2fd43dbeac06", "18322c0c-17f9-4725-96d0-582263bb78a1" });
        }
    }
}
