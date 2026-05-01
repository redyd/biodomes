using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class trackbiomespeciebytime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_biomes_species",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.DropColumn(
                name: "Count",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "biomes_species",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "Inserted",
                schema: "dbo",
                table: "biomes_species",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_biomes_species",
                schema: "dbo",
                table: "biomes_species",
                column: "Id");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c3bc10a5-c61f-4071-979c-77dc94386816", "aac15b89-6018-4bcc-8d36-38878740e13f" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "efc7803a-3723-40f5-b150-a61497ed9ea8", "b6f6f22e-ae9e-41ed-9a16-bda13de92879" });

            migrationBuilder.CreateIndex(
                name: "IX_biomes_species_BiomeId",
                schema: "dbo",
                table: "biomes_species",
                column: "BiomeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_biomes_species",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.DropIndex(
                name: "IX_biomes_species_BiomeId",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.DropColumn(
                name: "Inserted",
                schema: "dbo",
                table: "biomes_species");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                schema: "dbo",
                table: "biomes_species",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_biomes_species",
                schema: "dbo",
                table: "biomes_species",
                columns: new[] { "BiomeId", "SpecieId" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6e38db7b-1af9-44cf-b4e2-143c2d9289fd", "5e9203b5-cfb3-4a18-989d-4f52b7fe4288" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "66696b49-8cba-410d-a0a8-53a621dc9318", "bcc8c646-7249-44eb-aac0-27f13251bd4b" });
        }
    }
}
