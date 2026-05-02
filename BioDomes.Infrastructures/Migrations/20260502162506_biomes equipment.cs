using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class biomesequipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "biome_equipments",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "biomes_equipments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Inserted = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    BiomeId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_biomes_equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_biomes_equipments_biomes_BiomeId",
                        column: x => x.BiomeId,
                        principalSchema: "dbo",
                        principalTable: "biomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_biomes_equipments_equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalSchema: "dbo",
                        principalTable: "equipments",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "f9766240-fc1d-44f6-9387-fdd480bf9f84", "2a32a8bc-61e1-4456-a77b-b00862d0bfdb" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4cdeee44-ef68-4654-83f1-d99c51d9c2d2", "3734b1c3-1294-4160-aad3-8df2d6455a5d" });

            migrationBuilder.CreateIndex(
                name: "IX_biomes_equipments_BiomeId",
                schema: "dbo",
                table: "biomes_equipments",
                column: "BiomeId");

            migrationBuilder.CreateIndex(
                name: "IX_biomes_equipments_EquipmentId",
                schema: "dbo",
                table: "biomes_equipments",
                column: "EquipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "biomes_equipments",
                schema: "dbo");

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

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ed630374-122b-493b-bec0-863fea91242e", "e5b131f4-ced6-4b39-987d-8f03a7a5d4bc" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c4a87ce5-dc36-4d3e-a9f3-674837ae94bc", "ddd0801d-4cd3-450a-8844-7ddec16e5bd0" });

            migrationBuilder.CreateIndex(
                name: "IX_biome_equipments_EquipmentsId",
                schema: "dbo",
                table: "biome_equipments",
                column: "EquipmentsId");
        }
    }
}
