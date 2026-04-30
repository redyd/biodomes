using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class addcounttobiomespecie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_biomes_species_biomes_BiomesId",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.DropForeignKey(
                name: "FK_biomes_species_species_SpeciesSlug",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.RenameColumn(
                name: "SpeciesSlug",
                schema: "dbo",
                table: "biomes_species",
                newName: "SpecieSlug");

            migrationBuilder.RenameColumn(
                name: "BiomesId",
                schema: "dbo",
                table: "biomes_species",
                newName: "BiomeId");

            migrationBuilder.RenameIndex(
                name: "IX_biomes_species_SpeciesSlug",
                schema: "dbo",
                table: "biomes_species",
                newName: "IX_biomes_species_SpecieSlug");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                schema: "dbo",
                table: "biomes_species",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "21d126cc-90a6-4c9e-b881-1af503f3455f", "aeeb86a2-b3ff-40ce-ba07-f4ddd7784f04" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "62c1a059-d97d-488e-a26d-364f7f86101e", "c37f5475-3ad7-4f90-ad39-bc2f08eb6363" });

            migrationBuilder.AddForeignKey(
                name: "FK_biomes_species_biomes_BiomeId",
                schema: "dbo",
                table: "biomes_species",
                column: "BiomeId",
                principalSchema: "dbo",
                principalTable: "biomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_biomes_species_species_SpecieSlug",
                schema: "dbo",
                table: "biomes_species",
                column: "SpecieSlug",
                principalSchema: "dbo",
                principalTable: "species",
                principalColumn: "Slug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_biomes_species_biomes_BiomeId",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.DropForeignKey(
                name: "FK_biomes_species_species_SpecieSlug",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.DropColumn(
                name: "Count",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.RenameColumn(
                name: "SpecieSlug",
                schema: "dbo",
                table: "biomes_species",
                newName: "SpeciesSlug");

            migrationBuilder.RenameColumn(
                name: "BiomeId",
                schema: "dbo",
                table: "biomes_species",
                newName: "BiomesId");

            migrationBuilder.RenameIndex(
                name: "IX_biomes_species_SpecieSlug",
                schema: "dbo",
                table: "biomes_species",
                newName: "IX_biomes_species_SpeciesSlug");

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

            migrationBuilder.AddForeignKey(
                name: "FK_biomes_species_biomes_BiomesId",
                schema: "dbo",
                table: "biomes_species",
                column: "BiomesId",
                principalSchema: "dbo",
                principalTable: "biomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_biomes_species_species_SpeciesSlug",
                schema: "dbo",
                table: "biomes_species",
                column: "SpeciesSlug",
                principalSchema: "dbo",
                principalTable: "species",
                principalColumn: "Slug",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
