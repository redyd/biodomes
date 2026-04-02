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
            migrationBuilder.CreateTable(
                name: "SpeciesDiets",
                columns: table => new
                {
                    Label = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesDiets", x => x.Label);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesTypes",
                columns: table => new
                {
                    Label = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesTypes", x => x.Label);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeLabel = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DietLabel = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Slug);
                    table.ForeignKey(
                        name: "FK_Species_SpeciesDiets_DietLabel",
                        column: x => x.DietLabel,
                        principalTable: "SpeciesDiets",
                        principalColumn: "Label",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Species_SpeciesTypes_TypeLabel",
                        column: x => x.TypeLabel,
                        principalTable: "SpeciesTypes",
                        principalColumn: "Label",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "SpeciesDiets",
                column: "Label",
                values: new object[]
                {
                    "Carnivore",
                    "Herbivore",
                    "Omnivore",
                    "Photosynthèse"
                });

            migrationBuilder.InsertData(
                table: "SpeciesTypes",
                column: "Label",
                values: new object[]
                {
                    "Mammifère",
                    "Oiseau",
                    "Plante",
                    "Poisson"
                });

            migrationBuilder.InsertData(
                table: "Species",
                columns: new[] { "Slug", "DietLabel", "Height", "ImageUrl", "Name", "TypeLabel" },
                values: new object[,]
                {
                    { "aigle", "Carnivore", 70, null, "Aigle", "Oiseau" },
                    { "bambou", "Photosynthèse", 1000, null, "Bambou", "Plante" },
                    { "chene", "Photosynthèse", 2000, null, "Chêne", "Plante" },
                    { "lapin", "Herbivore", 35, null, "Lapin", "Mammifère" },
                    { "lion", "Carnivore", 120, null, "Lion", "Mammifère" },
                    { "ours", "Omnivore", 180, null, "Ours", "Mammifère" },
                    { "poulet", "Omnivore", 45, null, "Poulet", "Oiseau" },
                    { "renard", "Omnivore", 40, null, "Renard", "Mammifère" },
                    { "requin", "Carnivore", 500, null, "Requin", "Poisson" },
                    { "vache", "Herbivore", 150, null, "Vache", "Mammifère" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Species_DietLabel",
                table: "Species",
                column: "DietLabel");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Slug",
                table: "Species",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_TypeLabel",
                table: "Species",
                column: "TypeLabel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "SpeciesDiets");

            migrationBuilder.DropTable(
                name: "SpeciesTypes");
        }
    }
}
