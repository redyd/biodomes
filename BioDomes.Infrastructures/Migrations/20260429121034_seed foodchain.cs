using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class seedfoodchain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "specie_food_chains",
                columns: new[] { "PredatorId", "PreyId" },
                values: new object[,]
                {
                    { "araignee", "mouche" },
                    { "araignee", "souris" },
                    { "boa", "hirondelle" },
                    { "boa", "souris" },
                    { "hirondelle", "araignee" },
                    { "hirondelle", "mouche" },
                    { "lievre", "hetre" },
                    { "lievre", "paturin-des-pres" },
                    { "lion", "lievre" },
                    { "lion", "souris" },
                    { "souris", "chene" },
                    { "souris", "mouche" },
                    { "souris", "paturin-des-pres" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "araignee", "mouche" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "araignee", "souris" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "boa", "hirondelle" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "boa", "souris" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "hirondelle", "araignee" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "hirondelle", "mouche" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "lievre", "hetre" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "lievre", "paturin-des-pres" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "lion", "lievre" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "lion", "souris" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "souris", "chene" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "souris", "mouche" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "specie_food_chains",
                keyColumns: new[] { "PredatorId", "PreyId" },
                keyValues: new object[] { "souris", "paturin-des-pres" });

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
        }
    }
}
