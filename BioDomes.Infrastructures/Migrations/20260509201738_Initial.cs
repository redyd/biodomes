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
                name: "specie_classifications",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequireFood = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specie_classifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "specie_diets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specie_diets", x => x.Id);
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
                name: "biomes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Humidity = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_biomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_biomes_AspNetUsers_UserId",
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

            migrationBuilder.CreateTable(
                name: "species",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    UploadedImageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DietId = table.Column<int>(type: "int", nullable: false),
                    ClassificationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_species", x => x.Id);
                    table.ForeignKey(
                        name: "FK_species_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_species_specie_classifications_ClassificationId",
                        column: x => x.ClassificationId,
                        principalSchema: "dbo",
                        principalTable: "specie_classifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_species_specie_diets_DietId",
                        column: x => x.DietId,
                        principalSchema: "dbo",
                        principalTable: "specie_diets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "biomes_species",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Inserted = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    BiomeId = table.Column<int>(type: "int", nullable: false),
                    SpecieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_biomes_species", x => x.Id);
                    table.ForeignKey(
                        name: "FK_biomes_species_biomes_BiomeId",
                        column: x => x.BiomeId,
                        principalSchema: "dbo",
                        principalTable: "biomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_biomes_species_species_SpecieId",
                        column: x => x.SpecieId,
                        principalSchema: "dbo",
                        principalTable: "species",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "specie_food_chains",
                schema: "dbo",
                columns: table => new
                {
                    PredatorId = table.Column<int>(type: "int", nullable: false),
                    PreyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specie_food_chains", x => new { x.PredatorId, x.PreyId });
                    table.ForeignKey(
                        name: "FK_specie_food_chains_species_PredatorId",
                        column: x => x.PredatorId,
                        principalSchema: "dbo",
                        principalTable: "species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_specie_food_chains_species_PreyId",
                        column: x => x.PreyId,
                        principalSchema: "dbo",
                        principalTable: "species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Birthdate", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImageUrl", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SearchOrganism", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user-1", 0, new DateOnly(1990, 1, 1), "dd846e1f-d948-420b-af1f-4c36fb9ba85d", "admin@biodomes.com", true, "1637ca51-6139-4ec4-aa5a-3e824212b0e3.jpg", false, null, "ADMIN@BIODOMES.COM", "ADMIN", null, null, false, "Saint Paul", "8937fe83-549e-493f-a4b6-79f1ae4fdb94", false, "admin" },
                    { "user-2", 0, new DateOnly(1995, 6, 15), "2c59a1a3-9e94-46c6-9824-4da3fef7a162", "john@biodomes.com", true, null, false, null, "JOHN@BIODOMES.COM", "JOHN", null, null, false, "Marie Thérèse", "ad5877b3-25a8-4dbc-ba2e-4f6ea086f57d", false, "john" }
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
                table: "specie_classifications",
                columns: new[] { "Id", "Label", "RequireFood" },
                values: new object[,]
                {
                    { 1, "Mammifère", true },
                    { 2, "Oiseau", true },
                    { 3, "Poisson", true },
                    { 4, "Reptile", true },
                    { 5, "Amphibien", true },
                    { 6, "Crustacé", true },
                    { 7, "Arachnides", true },
                    { 8, "Insecte", true },
                    { 9, "Myriapode", true },
                    { 10, "Mollusque", true },
                    { 11, "Echinoderme", true },
                    { 12, "Anélide", true },
                    { 13, "Cnidaire", true },
                    { 14, "Plathelminthe", true },
                    { 15, "Porifère", true },
                    { 16, "Plante", false },
                    { 17, "Mousse", false },
                    { 18, "Algue", false },
                    { 19, "Lichen", false },
                    { 20, "Champignon", false }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "specie_diets",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "Carnivore" },
                    { 2, "Herbivore" },
                    { 3, "Omnivore" },
                    { 4, "Coprophages" },
                    { 5, "Limivores" },
                    { 6, "Détritivores" },
                    { 7, "Autotrophie (Photosynthèse)" },
                    { 8, "Hétérotrophie" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "biomes",
                columns: new[] { "Id", "Humidity", "ImageUrl", "IsPublic", "Name", "Temperature", "UserId" },
                values: new object[] { 1, 16.0, null, true, "Biotest Ultimate", 25.0, "user-1" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "classification_eaten_by_diet",
                columns: new[] { "classification_id", "diet_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 1, 8 },
                    { 2, 1 },
                    { 2, 3 },
                    { 2, 8 },
                    { 3, 1 },
                    { 3, 3 },
                    { 3, 8 },
                    { 4, 1 },
                    { 4, 3 },
                    { 4, 8 },
                    { 5, 1 },
                    { 5, 3 },
                    { 5, 8 },
                    { 6, 1 },
                    { 6, 3 },
                    { 6, 8 },
                    { 7, 1 },
                    { 7, 3 },
                    { 7, 8 },
                    { 8, 1 },
                    { 8, 3 },
                    { 8, 8 },
                    { 9, 1 },
                    { 9, 3 },
                    { 9, 8 },
                    { 10, 1 },
                    { 10, 3 },
                    { 10, 4 },
                    { 10, 5 },
                    { 10, 8 },
                    { 11, 1 },
                    { 11, 3 },
                    { 11, 8 },
                    { 12, 1 },
                    { 12, 3 },
                    { 12, 4 },
                    { 12, 5 },
                    { 12, 8 },
                    { 13, 1 },
                    { 13, 3 },
                    { 13, 8 },
                    { 14, 1 },
                    { 14, 3 },
                    { 14, 8 },
                    { 15, 1 },
                    { 15, 3 },
                    { 15, 8 },
                    { 16, 2 },
                    { 16, 3 },
                    { 16, 8 },
                    { 17, 2 },
                    { 17, 3 },
                    { 17, 8 },
                    { 18, 2 },
                    { 18, 3 },
                    { 18, 8 },
                    { 19, 2 },
                    { 19, 4 },
                    { 19, 6 },
                    { 20, 3 },
                    { 20, 4 },
                    { 20, 6 }
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

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "species",
                columns: new[] { "Id", "ClassificationId", "DietId", "Height", "Name", "Slug", "UploadedImageId", "UserId", "Weight" },
                values: new object[] { 1, 1, 1, 180, "Lion", "lion", "3d128e91-b947-4bf7-b988-dc8ffd1de92f.jpg", "user-1", 150000 });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "species",
                columns: new[] { "Id", "ClassificationId", "DietId", "Height", "IsPublic", "Name", "Slug", "UploadedImageId", "UserId", "Weight" },
                values: new object[] { 2, 4, 1, 225, true, "Boa", "boa", "47e0e461-066e-404f-8738-361d268f6219.jpg", "user-2", 13000 });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "species",
                columns: new[] { "Id", "ClassificationId", "DietId", "Height", "Name", "Slug", "UploadedImageId", "UserId", "Weight" },
                values: new object[,]
                {
                    { 3, 16, 7, 600, "Hêtre", "hetre", "9f21e809-9eeb-4714-8cd0-14b78d14fb60.jpg", "user-1", 830000 },
                    { 4, 16, 7, 2500, "Chêne", "chene", null, "user-2", 2100000 },
                    { 5, 8, 4, 1, "Mouche", "mouche", "2b101f74-b1f0-4c18-a183-0331f7a4788c.jpg", "user-1", 1 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "species",
                columns: new[] { "Id", "ClassificationId", "DietId", "Height", "IsPublic", "Name", "Slug", "UploadedImageId", "UserId", "Weight" },
                values: new object[,]
                {
                    { 6, 7, 1, 4, true, "Araignée", "araignee", "623fbf44-7418-44d7-8098-6cc6c7122c27.jpg", "user-1", 95 },
                    { 7, 2, 1, 18, true, "Hirondelle", "hirondelle", null, "user-1", 45 },
                    { 8, 1, 2, 50, true, "Lièvre", "lievre", "7f084f4d-de3c-4436-b359-6fdfd4b93ec8.jpg", "user-2", 4000 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "species",
                columns: new[] { "Id", "ClassificationId", "DietId", "Height", "Name", "Slug", "UploadedImageId", "UserId", "Weight" },
                values: new object[] { 9, 16, 7, 23, "Pâturin des prés", "paturin-des-pres", null, "user-1", 1 });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "species",
                columns: new[] { "Id", "ClassificationId", "DietId", "Height", "IsPublic", "Name", "Slug", "UploadedImageId", "UserId", "Weight" },
                values: new object[] { 10, 1, 3, 9, true, "Souris", "souris", "7e6d0ba0-7be3-4f94-a929-e7c04d3e1d90.jpg", "user-2", 25 });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "biomes_equipments",
                columns: new[] { "Id", "BiomeId", "EquipmentId" },
                values: new object[,]
                {
                    { 1, 1, 3 },
                    { 2, 1, 3 },
                    { 3, 1, 1 },
                    { 4, 1, 1 },
                    { 5, 1, 1 },
                    { 6, 1, 6 },
                    { 7, 1, 6 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "biomes_species",
                columns: new[] { "Id", "BiomeId", "SpecieId" },
                values: new object[,]
                {
                    { 1, 1, 2 },
                    { 11, 1, 3 },
                    { 12, 1, 3 },
                    { 13, 1, 3 },
                    { 14, 1, 3 },
                    { 15, 1, 3 },
                    { 16, 1, 3 },
                    { 17, 1, 3 },
                    { 18, 1, 3 },
                    { 19, 1, 3 },
                    { 21, 1, 5 },
                    { 22, 1, 5 },
                    { 23, 1, 5 },
                    { 24, 1, 5 },
                    { 25, 1, 5 },
                    { 31, 1, 7 },
                    { 32, 1, 7 },
                    { 33, 1, 7 },
                    { 34, 1, 7 },
                    { 41, 1, 10 },
                    { 42, 1, 10 },
                    { 43, 1, 10 },
                    { 44, 1, 10 },
                    { 45, 1, 10 },
                    { 46, 1, 10 },
                    { 47, 1, 10 },
                    { 48, 1, 10 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "specie_food_chains",
                columns: new[] { "PredatorId", "PreyId" },
                values: new object[,]
                {
                    { 1, 8 },
                    { 1, 10 },
                    { 2, 5 },
                    { 2, 10 },
                    { 5, 1 },
                    { 5, 8 },
                    { 6, 4 },
                    { 6, 9 },
                    { 7, 6 },
                    { 7, 10 },
                    { 10, 3 },
                    { 10, 8 },
                    { 10, 9 }
                });

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
                name: "IX_biomes_UserId",
                schema: "dbo",
                table: "biomes",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_biomes_species_BiomeId",
                schema: "dbo",
                table: "biomes_species",
                column: "BiomeId");

            migrationBuilder.CreateIndex(
                name: "IX_biomes_species_SpecieId",
                schema: "dbo",
                table: "biomes_species",
                column: "SpecieId");

            migrationBuilder.CreateIndex(
                name: "IX_classification_eaten_by_diet_diet_id",
                schema: "dbo",
                table: "classification_eaten_by_diet",
                column: "diet_id");

            migrationBuilder.CreateIndex(
                name: "IX_equipments_ConsumedResourceId",
                schema: "dbo",
                table: "equipments",
                column: "ConsumedResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_equipments_Name",
                schema: "dbo",
                table: "equipments",
                column: "Name",
                unique: true);

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
                name: "IX_specie_food_chains_PreyId",
                schema: "dbo",
                table: "specie_food_chains",
                column: "PreyId");

            migrationBuilder.CreateIndex(
                name: "IX_species_ClassificationId",
                schema: "dbo",
                table: "species",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_species_DietId",
                schema: "dbo",
                table: "species",
                column: "DietId");

            migrationBuilder.CreateIndex(
                name: "IX_species_Id",
                schema: "dbo",
                table: "species",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_species_Slug",
                schema: "dbo",
                table: "species",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_species_UserId",
                schema: "dbo",
                table: "species",
                column: "UserId");
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
                name: "biomes_equipments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "biomes_species",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "classification_eaten_by_diet",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "IdentityRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "specie_food_chains",
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
                name: "equipment_resources",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "specie_classifications",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "specie_diets",
                schema: "dbo");
        }
    }
}
