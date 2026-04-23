using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class classificationanddiet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DietLabel",
                schema: "dbo",
                table: "species");

            migrationBuilder.DropColumn(
                name: "TypeLabel",
                schema: "dbo",
                table: "species");

            migrationBuilder.AddColumn<int>(
                name: "ClassificationId",
                schema: "dbo",
                table: "species",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DietId",
                schema: "dbo",
                table: "species",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "specie_classifications",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specie_classifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "specie_diets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specie_diets", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "43ec93d7-0bc8-4f71-bda6-caa8b7a7daba", "d58c1500-54f4-4d36-b9da-6e5c0eb85ebe" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "5def1c4e-7c52-4c86-b519-e13cc39f0994", "2f87cebb-3bee-4190-9a77-8402aff40512" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "specie_classifications",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "Mammifère" },
                    { 2, "Oiseau" },
                    { 3, "Poisson" },
                    { 4, "Reptile" },
                    { 5, "Amphibien" },
                    { 6, "Crustacé" },
                    { 7, "Arachnides" },
                    { 8, "Insecte" },
                    { 9, "Myriapode" },
                    { 10, "Mollusque" },
                    { 11, "Echinoderme" },
                    { 12, "Anélide" },
                    { 13, "Cnidaire" },
                    { 14, "Plathelminthe" },
                    { 15, "Porifère" },
                    { 16, "Plante" },
                    { 17, "Mousse" },
                    { 18, "Algue" },
                    { 19, "Lichen" },
                    { 20, "Champignon" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "specie_diets",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "Carnivore" },
                    { 2, "Herbivore" },
                    { 3, "Omnivore" },
                    { 4, "Coprophages" },
                    { 5, "Limivores" },
                    { 6, "Détritivores" },
                    { 7, "Autotrophie (Photosynthèse)" },
                    { 8, "Hétérotrophie" }
                });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "araignee",
                columns: new[] { "ClassificationId", "DietId" },
                values: new object[] { 7, 1 });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "boa",
                columns: new[] { "ClassificationId", "DietId" },
                values: new object[] { 4, 1 });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "chene",
                columns: new[] { "ClassificationId", "DietId", "Name" },
                values: new object[] { 16, 7, "Chêne" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "hetre",
                columns: new[] { "ClassificationId", "DietId", "Name" },
                values: new object[] { 16, 7, "Hêtre" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "hirondelle",
                columns: new[] { "ClassificationId", "DietId" },
                values: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "lievre",
                columns: new[] { "ClassificationId", "DietId" },
                values: new object[] { 1, 2 });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "lion",
                columns: new[] { "ClassificationId", "DietId" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "mouche",
                columns: new[] { "ClassificationId", "DietId" },
                values: new object[] { 8, 4 });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "paturin-des-pres",
                columns: new[] { "ClassificationId", "DietId" },
                values: new object[] { 16, 7 });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "souris",
                columns: new[] { "ClassificationId", "DietId" },
                values: new object[] { 1, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_species_ClassificationId",
                schema: "dbo",
                table: "species",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_species_DietId",
                schema: "dbo",
                table: "species",
                column: "DietId");

            migrationBuilder.AddForeignKey(
                name: "FK_species_specie_classifications_ClassificationId",
                schema: "dbo",
                table: "species",
                column: "ClassificationId",
                principalSchema: "dbo",
                principalTable: "specie_classifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_species_specie_diets_DietId",
                schema: "dbo",
                table: "species",
                column: "DietId",
                principalSchema: "dbo",
                principalTable: "specie_diets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_species_specie_classifications_ClassificationId",
                schema: "dbo",
                table: "species");

            migrationBuilder.DropForeignKey(
                name: "FK_species_specie_diets_DietId",
                schema: "dbo",
                table: "species");

            migrationBuilder.DropTable(
                name: "specie_classifications",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "specie_diets",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_species_ClassificationId",
                schema: "dbo",
                table: "species");

            migrationBuilder.DropIndex(
                name: "IX_species_DietId",
                schema: "dbo",
                table: "species");

            migrationBuilder.DropColumn(
                name: "ClassificationId",
                schema: "dbo",
                table: "species");

            migrationBuilder.DropColumn(
                name: "DietId",
                schema: "dbo",
                table: "species");

            migrationBuilder.AddColumn<string>(
                name: "DietLabel",
                schema: "dbo",
                table: "species",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypeLabel",
                schema: "dbo",
                table: "species",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "1cb239c3-a7d3-4632-824a-5eda447d77a7", "dd3c3d7d-3eb0-4afb-a5a6-686014111591" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c873a04c-c345-4f75-976c-f4a91e0a27e0", "6d3c49d6-ee79-4035-a58c-f1fb431f4a16" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "araignee",
                columns: new[] { "DietLabel", "TypeLabel" },
                values: new object[] { "Carnivore", "Arachnides" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "boa",
                columns: new[] { "DietLabel", "TypeLabel" },
                values: new object[] { "Carnivore", "Reptile" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "chene",
                columns: new[] { "DietLabel", "Name", "TypeLabel" },
                values: new object[] { "Photosynthèse", "Chene", "Plante" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "hetre",
                columns: new[] { "DietLabel", "Name", "TypeLabel" },
                values: new object[] { "Photosynthèse", "Hetre", "Plante" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "hirondelle",
                columns: new[] { "DietLabel", "TypeLabel" },
                values: new object[] { "Carnivore", "Oiseau" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "lievre",
                columns: new[] { "DietLabel", "TypeLabel" },
                values: new object[] { "Herbivore", "Mammifère" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "lion",
                columns: new[] { "DietLabel", "TypeLabel" },
                values: new object[] { "Carnivore", "Mammifère" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "mouche",
                columns: new[] { "DietLabel", "TypeLabel" },
                values: new object[] { "Coprophage", "Insecte" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "paturin-des-pres",
                columns: new[] { "DietLabel", "TypeLabel" },
                values: new object[] { "Photosynthèse", "Plante" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "species",
                keyColumn: "Slug",
                keyValue: "souris",
                columns: new[] { "DietLabel", "TypeLabel" },
                values: new object[] { "Omnivore", "Mammifère" });
        }
    }
}
