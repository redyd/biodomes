using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class fixclassification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "RequireFood",
                schema: "dbo",
                table: "specie_classifications",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

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

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 16,
                column: "RequireFood",
                value: false);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 17,
                column: "RequireFood",
                value: false);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 18,
                column: "RequireFood",
                value: false);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 19,
                column: "RequireFood",
                value: false);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 20,
                column: "RequireFood",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "RequireFood",
                schema: "dbo",
                table: "specie_classifications",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "fef09869-1c01-4612-a5a9-0849bd0035d4", "8ae30d4c-2542-4d7f-a28c-0e060ffb9fcf" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a22c9408-01b7-426d-b297-71739ad49deb", "9121d3d0-4d6f-41ae-8183-9b0c0045a2ea" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 16,
                column: "RequireFood",
                value: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 17,
                column: "RequireFood",
                value: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 18,
                column: "RequireFood",
                value: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 19,
                column: "RequireFood",
                value: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "specie_classifications",
                keyColumn: "Id",
                keyValue: 20,
                column: "RequireFood",
                value: true);
        }
    }
}
