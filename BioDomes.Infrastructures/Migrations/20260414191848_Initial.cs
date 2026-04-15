using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Birthdate = table.Column<DateOnly>(type: "date", nullable: false),
                    SearchOrganism = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

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
                name: "IdentityRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "species",
                schema: "dbo",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    UploadedImageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DietLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_species", x => x.Slug);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "dbo",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProducedResourceId = table.Column<int>(type: "int", nullable: false),
                    ConsumedResourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_equipments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Birthdate", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImageUrl", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SearchOrganism", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user-1", 0, new DateOnly(1990, 1, 1), "6bf0aec1-2435-4330-bc31-57588915c073", "admin@biodomes.com", true, null, false, null, "ADMIN@BIODOMES.COM", "ADMIN", null, null, false, "Saint Paul", "97cdbea4-3f27-487e-9a3b-eccc88a1c727", false, "admin" },
                    { "user-2", 0, new DateOnly(1995, 6, 15), "5e7707ef-0cc6-4a07-837c-4920c95ab471", "john@biodomes.com", true, null, false, null, "JOHN@BIODOMES.COM", "JOHN", null, null, false, "Marie Thérèse", "34c64448-aa75-42a9-92c3-ab9570572bb0", false, "john" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "IdentityRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "admin-role-id", null, "Admin", "ADMIN" });

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
                    { 10, "Magnésium" },
                    { 11, "Énergie" },
                    { 12, "Matière organique" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "species",
                columns: new[] { "Slug", "DietLabel", "Height", "Name", "TypeLabel", "UploadedImageId", "Weight" },
                values: new object[,]
                {
                    { "araignee", "Carnivore", 4, "Araignée", "Arachnides", "623fbf44-7418-44d7-8098-6cc6c7122c27.jpg", 95 },
                    { "boa", "Carnivore", 225, "Boa", "Reptile", "47e0e461-066e-404f-8738-361d268f6219.jpg", 13000 },
                    { "chene", "Photosynthèse", 2500, "Chene", "Plante", null, 2100000 },
                    { "hetre", "Photosynthèse", 600, "Hetre", "Plante", "9f21e809-9eeb-4714-8cd0-14b78d14fb60.jpg", 830000 },
                    { "hirondelle", "Carnivore", 18, "Hirondelle", "Oiseau", null, 45 },
                    { "lievre", "Herbivore", 50, "Lièvre", "Mammifère", "7f084f4d-de3c-4436-b359-6fdfd4b93ec8.jpg", 4000 },
                    { "lion", "Carnivore", 180, "Lion", "Mammifère", "3d128e91-b947-4bf7-b988-dc8ffd1de92f.jpg", 150000 },
                    { "mouche", "Coprophage", 1, "Mouche", "Insecte", "2b101f74-b1f0-4c18-a183-0331f7a4788c.jpg", 1 },
                    { "paturin-des-pres", "Photosynthèse", 23, "Pâturin des prés", "Plante", null, 1 },
                    { "souris", "Omnivore", 9, "Souris", "Mammifère", "7e6d0ba0-7be3-4f94-a929-e7c04d3e1d90.jpg", 25 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "equipments",
                columns: new[] { "Id", "ConsumedResourceId", "Image", "Name", "ProducedResourceId", "UserId" },
                values: new object[] { 1, 11, "ca0fbc00-841b-41bb-8329-b0c95931d24e.png", "Générateur de lumière artificielle", 1, "user-1" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "equipments",
                columns: new[] { "Id", "ConsumedResourceId", "Image", "IsPublic", "Name", "ProducedResourceId", "UserId" },
                values: new object[] { 2, 5, "e7a73200-4110-492e-ad32-e10c6654995b.png", true, "Condenseur atmosphérique", 2, "user-1" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "equipments",
                columns: new[] { "Id", "ConsumedResourceId", "Image", "Name", "ProducedResourceId", "UserId" },
                values: new object[] { 3, 5, "2fa0bd7e-1425-4359-a841-9b6a59b1d3f6.png", "Extracteur de CO2", 6, "user-1" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "equipments",
                columns: new[] { "Id", "ConsumedResourceId", "Image", "IsPublic", "Name", "ProducedResourceId", "UserId" },
                values: new object[] { 4, 6, "bff55fcc-19e0-4c59-9575-6689dedb8d05.png", true, "Module de photosynthèse", 5, "user-1" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "equipments",
                columns: new[] { "Id", "ConsumedResourceId", "Image", "Name", "ProducedResourceId", "UserId" },
                values: new object[] { 5, 3, "1056b6e4-fa44-4d5d-a309-13e7a7bd9999.png", "Synthétiseur d'engrais", 7, "user-2" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "equipments",
                columns: new[] { "Id", "ConsumedResourceId", "Image", "IsPublic", "Name", "ProducedResourceId", "UserId" },
                values: new object[] { 6, 12, "a430eda5-2206-4963-8f6e-c9731c0d6bef.png", true, "Recycleur de biomasse", 3, "user-2" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "equipments",
                columns: new[] { "Id", "ConsumedResourceId", "Image", "Name", "ProducedResourceId", "UserId" },
                values: new object[] { 7, 2, "861decc8-a7e7-4b8c-8f31-76b3c09342b9.png", "Filtre minéral", 9, "user-2" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "equipments",
                columns: new[] { "Id", "ConsumedResourceId", "Image", "IsPublic", "Name", "ProducedResourceId", "UserId" },
                values: new object[] { 8, 2, "4fba83ed-a55f-4e1b-80b5-6829a696b7b3.png", true, "Extracteur d'électrolytes", 10, "user-2" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "dbo",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "dbo",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "dbo",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "dbo",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

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

            migrationBuilder.CreateIndex(
                name: "IX_equipments_UserId",
                schema: "dbo",
                table: "equipments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_species_Slug",
                schema: "dbo",
                table: "species",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "biome_equipments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "biomes_species",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "IdentityRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "equipments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "biomes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "species",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "equipment_resources",
                schema: "dbo");
        }
    }
}
