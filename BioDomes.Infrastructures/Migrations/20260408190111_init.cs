using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "equipment_resources",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipment_resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "species",
                schema: "dbo",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    UploadedImageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DietLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_species", x => x.Slug);
                });

            migrationBuilder.CreateTable(
                name: "equipments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ProducedResourceId = table.Column<int>(type: "int", nullable: false),
                    ConsumedResourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_equipments_equipment_resources_ConsumedResourceId",
                        column: x => x.ConsumedResourceId,
                        principalSchema: "dbo",
                        principalTable: "equipment_resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_equipments_equipment_resources_ProducedResourceId",
                        column: x => x.ProducedResourceId,
                        principalSchema: "dbo",
                        principalTable: "equipment_resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "equipment_resources",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "Lumière" },
                    { 2, "Eau" },
                    { 3, "Azote" },
                    { 4, "Hydrogène" },
                    { 5, "Oxygène" },
                    { 6, "CO2" },
                    { 7, "Phosphore" },
                    { 8, "Potassium" },
                    { 9, "Calcium" },
                    { 10, "Magnésium" },
                    { 11, "Énergie" },
                    { 12, "Matière organique" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "species",
                columns: new[] { "Slug", "DietLabel", "Height", "Name", "TypeLabel", "UploadedImageId", "Weight" },
                values: new object[,]
                {
                    { "araignee", "Carnivore", 4, "Araignée", "Arachnides", "623fbf44-7418-44d7-8098-6cc6c7122c27.jpg", 95 },
                    { "boa", "Carnivore", 225, "Boa", "Reptile", "47e0e461-066e-404f-8738-361d268f6219.jpg", 13000 },
                    { "chene", "Photosynthèse", 2500, "Chene", "Plante", null, 2100000 },
                    { "hetre", "Photosynthèse", 600, "Hetre", "Plante", "9f21e809-9eeb-4714-8cd0-14b78d14fb60.jpg", 830000 },
                    { "hirondelle", "Carnivore", 18, "Hirondelle", "Oiseau", null, 45 },
                    { "lievre", "Herbivore", 50, "Lièvre", "Mammifère", "7f084f4d-de3c-4436-b359-6fdfd4b93ec8.jpg", 4000 },
                    { "lion", "Carnivore", 180, "Lion", "Mammifère", "3d128e91-b947-4bf7-b988-dc8ffd1de92f.jpg", 150000 },
                    { "mouche", "Coprophage", 1, "Mouche", "Insecte", "2b101f74-b1f0-4c18-a183-0331f7a4788c.jpg", 1 },
                    { "paturin-des-pres", "Photosynthèse", 23, "Pâturin des prés", "Plante", null, 1 },
                    { "souris", "Omnivore", 9, "Souris", "Mammifère", "7e6d0ba0-7be3-4f94-a929-e7c04d3e1d90.jpg", 25 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "equipments",
                columns: new[] { "Id", "ConsumedResourceId", "Image", "Name", "ProducedResourceId" },
                values: new object[,]
                {
                    { 1, 11, "ca0fbc00-841b-41bb-8329-b0c95931d24e.png", "Générateur de lumière artificielle", 1 },
                    { 2, 5, "e7a73200-4110-492e-ad32-e10c6654995b.png", "Condenseur atmosphérique", 2 },
                    { 3, 5, "2fa0bd7e-1425-4359-a841-9b6a59b1d3f6.png", "Extracteur de CO2", 6 },
                    { 4, 6, "bff55fcc-19e0-4c59-9575-6689dedb8d05.png", "Module de photosynthèse", 5 },
                    { 5, 3, "1056b6e4-fa44-4d5d-a309-13e7a7bd9999.png", "Synthétiseur d'engrais", 7 },
                    { 6, 12, "a430eda5-2206-4963-8f6e-c9731c0d6bef.png", "Recycleur de biomasse", 3 },
                    { 7, 2, "861decc8-a7e7-4b8c-8f31-76b3c09342b9.png", "Filtre minéral", 9 },
                    { 8, 2, "4fba83ed-a55f-4e1b-80b5-6829a696b7b3.png", "Extracteur d'électrolytes", 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_equipments_ConsumedResourceId",
                schema: "dbo",
                table: "equipments",
                column: "ConsumedResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_equipments_ProducedResourceId",
                schema: "dbo",
                table: "equipments",
                column: "ProducedResourceId");

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
                name: "equipments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "species",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "equipment_resources",
                schema: "dbo");
        }
    }
}
