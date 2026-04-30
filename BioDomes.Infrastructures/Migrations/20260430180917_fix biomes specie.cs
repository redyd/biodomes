using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class fixbiomesspecie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_biomes_species_species_SpecieEntitySlug",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.DropIndex(
                name: "IX_biomes_species_SpecieEntitySlug",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.DropColumn(
                name: "SpecieEntitySlug",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e9e07f3e-1624-4b69-93a5-9439c03ba188", "d6c9f183-7b3c-45b2-878d-1ec157fd52f3" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "0174c210-fc2f-4ba2-921b-ecd8cc401cc4", "48911d41-01c7-4135-9261-284ce953dda9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpecieEntitySlug",
                schema: "dbo",
                table: "biomes_species",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "9e64884f-cb1d-4dfb-b7a8-907a54bd70d2", "ff73410c-b8f2-43d8-8f46-4a489125828b" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b392696f-d498-40dd-bde5-165c17c7d893", "e962a629-cf2d-411c-bb79-879f420ff69d" });

            migrationBuilder.CreateIndex(
                name: "IX_biomes_species_SpecieEntitySlug",
                schema: "dbo",
                table: "biomes_species",
                column: "SpecieEntitySlug");

            migrationBuilder.AddForeignKey(
                name: "FK_biomes_species_species_SpecieEntitySlug",
                schema: "dbo",
                table: "biomes_species",
                column: "SpecieEntitySlug",
                principalSchema: "dbo",
                principalTable: "species",
                principalColumn: "Slug");
        }
    }
}
