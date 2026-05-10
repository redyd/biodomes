using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class idtoslug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                schema: "dbo",
                table: "specie_diets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                schema: "dbo",
                table: "specie_classifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                schema: "dbo",
                table: "equipments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                schema: "dbo",
                table: "biomes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "biomes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Slug",
                value: "biotest-ultimate");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipments",
                keyColumn: "Id",
                keyValue: 1,
                column: "Slug",
                value: "generateur-de-lumiere-artificielle");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipments",
                keyColumn: "Id",
                keyValue: 2,
                column: "Slug",
                value: "condenseur-atmospherique");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipments",
                keyColumn: "Id",
                keyValue: 3,
                column: "Slug",
                value: "extracteur-de-co2");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipments",
                keyColumn: "Id",
                keyValue: 4,
                column: "Slug",
                value: "module-de-photosynthese");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipments",
                keyColumn: "Id",
                keyValue: 5,
                column: "Slug",
                value: "synthetiseur-d-engrais");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipments",
                keyColumn: "Id",
                keyValue: 6,
                column: "Slug",
                value: "recycleur-de-biomasse");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipments",
                keyColumn: "Id",
                keyValue: 7,
                column: "Slug",
                value: "filtre-mineral");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "equipments",
                keyColumn: "Id",
                keyValue: 8,
                column: "Slug",
                value: "extracteur-d-electrolytes");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 1,
                column: "Slug",
                value: "mammifere");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 2,
                column: "Slug",
                value: "oiseau");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 3,
                column: "Slug",
                value: "poisson");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 4,
                column: "Slug",
                value: "reptile");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 5,
                column: "Slug",
                value: "amphibien");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 6,
                column: "Slug",
                value: "crustace");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 7,
                column: "Slug",
                value: "arachnides");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 8,
                column: "Slug",
                value: "insecte");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 9,
                column: "Slug",
                value: "myriapode");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 10,
                column: "Slug",
                value: "mollusque");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 11,
                column: "Slug",
                value: "echinoderme");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 12,
                column: "Slug",
                value: "anelide");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 13,
                column: "Slug",
                value: "cnidaire");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 14,
                column: "Slug",
                value: "plathelminthe");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 15,
                column: "Slug",
                value: "porifere");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 16,
                column: "Slug",
                value: "plante");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 17,
                column: "Slug",
                value: "mousse");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 18,
                column: "Slug",
                value: "algue");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 19,
                column: "Slug",
                value: "lichen");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 20,
                column: "Slug",
                value: "champignon");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_diets",
                keyColumn: "Id",
                keyValue: 1,
                column: "Slug",
                value: "carnivore");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_diets",
                keyColumn: "Id",
                keyValue: 2,
                column: "Slug",
                value: "herbivore");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_diets",
                keyColumn: "Id",
                keyValue: 3,
                column: "Slug",
                value: "omnivore");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_diets",
                keyColumn: "Id",
                keyValue: 4,
                column: "Slug",
                value: "coprophages");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_diets",
                keyColumn: "Id",
                keyValue: 5,
                column: "Slug",
                value: "limivores");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_diets",
                keyColumn: "Id",
                keyValue: 6,
                column: "Slug",
                value: "detritivores");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_diets",
                keyColumn: "Id",
                keyValue: 7,
                column: "Slug",
                value: "autotrophie-photosynthese");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_diets",
                keyColumn: "Id",
                keyValue: 8,
                column: "Slug",
                value: "heterotrophie");

            migrationBuilder.CreateIndex(
                name: "IX_specie_diets_Slug",
                schema: "dbo",
                table: "specie_diets",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_specie_classifications_Slug",
                schema: "dbo",
                table: "specie_classifications",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_equipments_Slug",
                schema: "dbo",
                table: "equipments",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_biomes_Slug",
                schema: "dbo",
                table: "biomes",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_specie_diets_Slug",
                schema: "dbo",
                table: "specie_diets");

            migrationBuilder.DropIndex(
                name: "IX_specie_classifications_Slug",
                schema: "dbo",
                table: "specie_classifications");

            migrationBuilder.DropIndex(
                name: "IX_equipments_Slug",
                schema: "dbo",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "IX_biomes_Slug",
                schema: "dbo",
                table: "biomes");

            migrationBuilder.DropColumn(
                name: "Slug",
                schema: "dbo",
                table: "specie_diets");

            migrationBuilder.DropColumn(
                name: "Slug",
                schema: "dbo",
                table: "specie_classifications");

            migrationBuilder.DropColumn(
                name: "Slug",
                schema: "dbo",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "Slug",
                schema: "dbo",
                table: "biomes");

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

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-3",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "37cdf7c8-d666-4186-bc5a-17ae022e7f79", "0b0cb2d4-b0b5-4c43-b070-b8ede3ec81cc" });
        }
    }
}
