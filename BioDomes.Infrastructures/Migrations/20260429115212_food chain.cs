using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class foodchain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpecieEntitySlug",
                schema: "dbo",
                table: "biomes_species",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "specie_food_chains",
                schema: "dbo",
                columns: table => new
                {
                    PredatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PreyId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specie_food_chains", x => new { x.PredatorId, x.PreyId });
                    table.ForeignKey(
                        name: "FK_specie_food_chains_species_PredatorId",
                        column: x => x.PredatorId,
                        principalSchema: "dbo",
                        principalTable: "species",
                        principalColumn: "Slug",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_specie_food_chains_species_PreyId",
                        column: x => x.PreyId,
                        principalSchema: "dbo",
                        principalTable: "species",
                        principalColumn: "Slug",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "53cb523c-e618-4837-831e-cdaacd8e944a", "b0fca10c-8ef8-43de-bc03-208e77f37012" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bf9409a9-9747-4dba-a6c3-9bddfc7686d5", "7f070685-25dc-4c7e-a16e-ac5e91b1659c" });

            migrationBuilder.CreateIndex(
                name: "IX_biomes_species_SpecieEntitySlug",
                schema: "dbo",
                table: "biomes_species",
                column: "SpecieEntitySlug");

            migrationBuilder.CreateIndex(
                name: "IX_specie_food_chains_PreyId",
                schema: "dbo",
                table: "specie_food_chains",
                column: "PreyId");

            migrationBuilder.AddForeignKey(
                name: "FK_biomes_species_species_SpecieEntitySlug",
                schema: "dbo",
                table: "biomes_species",
                column: "SpecieEntitySlug",
                principalSchema: "dbo",
                principalTable: "species",
                principalColumn: "Slug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_biomes_species_species_SpecieEntitySlug",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.DropTable(
                name: "specie_food_chains",
                schema: "dbo");

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
                values: new object[] { "3884d28f-7c25-424c-81da-144699f38ef5", "4cd9f21e-0a3f-4e08-9f83-bda454dd15b5" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7464653e-0c62-40d7-bd18-c84e64242101", "61ae3a9e-aee5-4d4d-93bf-90453096d694" });
        }
    }
}
