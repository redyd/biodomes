using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class equipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "equipments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    { 10, "Magnésium" }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "equipments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "equipment_resources",
                schema: "dbo");
        }
    }
}
