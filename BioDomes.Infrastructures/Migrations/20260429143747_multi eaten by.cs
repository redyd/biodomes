using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class multieatenby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_specie_classifications_specie_diets_EatenTypeDietId",
                schema: "dbo",
                table: "specie_classifications");

            migrationBuilder.DropIndex(
                name: "IX_specie_classifications_EatenTypeDietId",
                schema: "dbo",
                table: "specie_classifications");

            migrationBuilder.DropColumn(
                name: "EatenTypeDietId",
                schema: "dbo",
                table: "specie_classifications");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                schema: "dbo",
                table: "specie_classifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "classification_eaten_by_diet",
                schema: "dbo",
                columns: table => new
                {
                    classification_id = table.Column<int>(type: "int", nullable: false),
                    diet_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classification_eaten_by_diet", x => new { x.classification_id, x.diet_id });
                    table.ForeignKey(
                        name: "FK_classification_eaten_by_diet_specie_classifications_classification_id",
                        column: x => x.classification_id,
                        principalSchema: "dbo",
                        principalTable: "specie_classifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_classification_eaten_by_diet_specie_diets_diet_id",
                        column: x => x.diet_id,
                        principalSchema: "dbo",
                        principalTable: "specie_diets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "9e64884f-cb1d-4dfb-b7a8-907a54bd70d2", "ff73410c-b8f2-43d8-8f46-4a489125828b" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b392696f-d498-40dd-bde5-165c17c7d893", "e962a629-cf2d-411c-bb79-879f420ff69d" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "classification_eaten_by_diet",
                columns: new[] { "classification_id", "diet_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 2, 1 },
                    { 2, 3 },
                    { 3, 1 },
                    { 3, 3 },
                    { 4, 1 },
                    { 4, 3 },
                    { 5, 1 },
                    { 5, 3 },
                    { 6, 1 },
                    { 6, 3 },
                    { 7, 1 },
                    { 7, 3 },
                    { 8, 1 },
                    { 8, 3 },
                    { 9, 1 },
                    { 9, 3 },
                    { 10, 1 },
                    { 10, 3 },
                    { 11, 1 },
                    { 11, 3 },
                    { 12, 1 },
                    { 12, 3 },
                    { 13, 1 },
                    { 13, 3 },
                    { 14, 1 },
                    { 14, 3 },
                    { 15, 1 },
                    { 15, 3 },
                    { 16, 2 },
                    { 16, 3 },
                    { 17, 2 },
                    { 17, 3 },
                    { 18, 2 },
                    { 18, 3 },
                    { 19, 2 },
                    { 19, 6 },
                    { 20, 3 },
                    { 20, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_classification_eaten_by_diet_diet_id",
                schema: "dbo",
                table: "classification_eaten_by_diet",
                column: "diet_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "classification_eaten_by_diet",
                schema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                schema: "dbo",
                table: "specie_classifications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "EatenTypeDietId",
                schema: "dbo",
                table: "specie_classifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4b7f9a01-01a2-40d3-ab77-30f9326ca98b", "0895568b-6e22-4ae8-bc86-2d36ff3654bd" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6a6648ea-947d-4122-bff4-83d8a3bdb822", "362cd948-917a-4534-8a8b-9f10e09f70a3" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 1,
                column: "EatenTypeDietId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 2,
                column: "EatenTypeDietId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 3,
                column: "EatenTypeDietId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 4,
                column: "EatenTypeDietId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 5,
                column: "EatenTypeDietId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 6,
                column: "EatenTypeDietId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 7,
                column: "EatenTypeDietId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 8,
                column: "EatenTypeDietId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 9,
                column: "EatenTypeDietId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 10,
                column: "EatenTypeDietId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 11,
                column: "EatenTypeDietId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 12,
                column: "EatenTypeDietId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 13,
                column: "EatenTypeDietId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 14,
                column: "EatenTypeDietId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 15,
                column: "EatenTypeDietId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 16,
                column: "EatenTypeDietId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 17,
                column: "EatenTypeDietId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 18,
                column: "EatenTypeDietId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 19,
                column: "EatenTypeDietId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 20,
                column: "EatenTypeDietId",
                value: 6);

            migrationBuilder.CreateIndex(
                name: "IX_specie_classifications_EatenTypeDietId",
                schema: "dbo",
                table: "specie_classifications",
                column: "EatenTypeDietId");

            migrationBuilder.AddForeignKey(
                name: "FK_specie_classifications_specie_diets_EatenTypeDietId",
                schema: "dbo",
                table: "specie_classifications",
                column: "EatenTypeDietId",
                principalSchema: "dbo",
                principalTable: "specie_diets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
