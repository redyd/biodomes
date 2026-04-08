using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class biomes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "biomes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Humidity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_biomes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "biome_equipments",
                schema: "dbo",
                columns: table => new
                {
                    BiomesId = table.Column<int>(type: "int", nullable: false),
                    EquipmentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_biome_equipments", x => new { x.BiomesId, x.EquipmentsId });
                    table.ForeignKey(
                        name: "FK_biome_equipments_biomes_BiomesId",
                        column: x => x.BiomesId,
                        principalSchema: "dbo",
                        principalTable: "biomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_biome_equipments_equipments_EquipmentsId",
                        column: x => x.EquipmentsId,
                        principalSchema: "dbo",
                        principalTable: "equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "biomes_species",
                schema: "dbo",
                columns: table => new
                {
                    BiomesId = table.Column<int>(type: "int", nullable: false),
                    SpeciesSlug = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_biomes_species", x => new { x.BiomesId, x.SpeciesSlug });
                    table.ForeignKey(
                        name: "FK_biomes_species_biomes_BiomesId",
                        column: x => x.BiomesId,
                        principalSchema: "dbo",
                        principalTable: "biomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_biomes_species_species_SpeciesSlug",
                        column: x => x.SpeciesSlug,
                        principalSchema: "dbo",
                        principalTable: "species",
                        principalColumn: "Slug",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_biome_equipments_EquipmentsId",
                schema: "dbo",
                table: "biome_equipments",
                column: "EquipmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_biomes_species_SpeciesSlug",
                schema: "dbo",
                table: "biomes_species",
                column: "SpeciesSlug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "biome_equipments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "biomes_species",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "biomes",
                schema: "dbo");
        }
    }
}
