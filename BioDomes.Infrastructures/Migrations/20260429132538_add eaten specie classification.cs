using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class addeatenspecieclassification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "9f72c6e4-f7d5-4e73-b2ef-cf3d3c9a7354", "e7608f2a-f76f-4d04-882c-5af61305fd61" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a62970dd-fce9-4277-a324-58ba1dbdf472", "c8d0e1a2-9126-4932-9729-b870c73443f9" });
        }
    }
}
