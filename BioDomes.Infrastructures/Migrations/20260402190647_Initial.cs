using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "species",
                schema: "dbo",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    UploadedImageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DietLabel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_species", x => x.Slug);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "species",
                columns: new[] { "Slug", "DietLabel", "Height", "Name", "TypeLabel", "UploadedImageId", "Weight" },
                values: new object[,]
                {
                    { "araignee", "Carnivore", 4, "Araignée", "Arachnides", null, 95 },
                    { "boa", "Carnivore", 225, "Boa", "Reptile", null, 13000 },
                    { "chene", "Photosynthèse", 2500, "Chene", "Plante", null, 2100000 },
                    { "hetre", "Photosynthèse", 600, "Hetre", "Plante", null, 830000 },
                    { "hirondelle", "Carnivore", 18, "Hirondelle", "Oiseau", null, 45 },
                    { "lievre", "Herbivore", 50, "Lièvre", "Mammifère", null, 4000 },
                    { "lion", "Carnivore", 180, "Lion", "Mammifère", null, 150000 },
                    { "mouche", "Coprophage", 1, "Mouche", "Insecte", null, 1 },
                    { "paturin-des-pres", "Photosynthèse", 23, "Pâturin des prés", "Plante", null, 1 },
                    { "souris", "Omnivore", 9, "Souris", "Mammifère", null, 25 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_species_Slug",
                schema: "dbo",
                table: "species",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "species",
                schema: "dbo");
        }
    }
}
