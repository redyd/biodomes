using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class ispublic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "species",
                type: "nvarchar(75)",
                maxLength: 75,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                schema: "dbo",
                table: "species",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EquipmentResourceEntityId",
                schema: "dbo",
                table: "equipments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                schema: "dbo",
                table: "equipments",
                type: "bit",
                nullable: false,
                defaultValue: false);
            
            migrationBuilder.CreateIndex(
                name: "IX_equipments_EquipmentResourceEntityId",
                schema: "dbo",
                table: "equipments",
                column: "EquipmentResourceEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_equipments_equipment_resources_EquipmentResourceEntityId",
                schema: "dbo",
                table: "equipments",
                column: "EquipmentResourceEntityId",
                principalSchema: "dbo",
                principalTable: "equipment_resources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_equipments_equipment_resources_EquipmentResourceEntityId",
                schema: "dbo",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "IX_equipments_EquipmentResourceEntityId",
                schema: "dbo",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                schema: "dbo",
                table: "species");

            migrationBuilder.DropColumn(
                name: "EquipmentResourceEntityId",
                schema: "dbo",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                schema: "dbo",
                table: "equipments");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "species",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(75)",
                oldMaxLength: 75);
        }
    }
}
