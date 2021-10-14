using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reproduction.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyBaseEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MyAbility_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackgroundId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EffectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsOptional = table.Column<bool>(type: "bit", nullable: true),
                    AbilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TraitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MyTrait_Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyBaseEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyBaseEntity_MyBaseEntity_AbilityId",
                        column: x => x.AbilityId,
                        principalTable: "MyBaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MyBaseEntity_MyBaseEntity_BackgroundId",
                        column: x => x.BackgroundId,
                        principalTable: "MyBaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MyBaseEntity_MyBaseEntity_EffectId",
                        column: x => x.EffectId,
                        principalTable: "MyBaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MyBaseEntity_MyBaseEntity_TraitId",
                        column: x => x.TraitId,
                        principalTable: "MyBaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MyBaseEntity",
                columns: new[] { "Id", "Discriminator", "MyAbility_Name" },
                values: new object[] { new Guid("f02c944c-9cd5-44b1-9567-eff6e051ada4"), "MyAbility", "SomeAbility" });

            migrationBuilder.InsertData(
                table: "MyBaseEntity",
                columns: new[] { "Id", "Discriminator", "Name" },
                values: new object[] { new Guid("8c316892-8b7d-47f8-9b96-b4da3418739c"), "MyBackground", "SomeBackground" });

            migrationBuilder.InsertData(
                table: "MyBaseEntity",
                columns: new[] { "Id", "Discriminator", "MyTrait_Name" },
                values: new object[] { new Guid("d421e715-73e7-413a-907a-bff501a53f58"), "MyTrait", "SomeTrait" });

            migrationBuilder.InsertData(
                table: "MyBaseEntity",
                columns: new[] { "Id", "AbilityId", "Discriminator", "IsOptional" },
                values: new object[] { new Guid("288e3587-7d6b-44c4-809d-872167d1940c"), new Guid("f02c944c-9cd5-44b1-9567-eff6e051ada4"), "MyAbilityEffect", false });

            migrationBuilder.InsertData(
                table: "MyBaseEntity",
                columns: new[] { "Id", "Discriminator", "IsOptional", "TraitId" },
                values: new object[] { new Guid("f7eb835e-4c39-4655-9545-61ced8bce356"), "MyTraitEffect", false, new Guid("d421e715-73e7-413a-907a-bff501a53f58") });

            migrationBuilder.InsertData(
                table: "MyBaseEntity",
                columns: new[] { "Id", "BackgroundId", "Discriminator", "EffectId" },
                values: new object[] { new Guid("638d2005-d651-4939-af03-04bc7717e9bd"), new Guid("8c316892-8b7d-47f8-9b96-b4da3418739c"), "MyBackgroundEffectBinding", new Guid("288e3587-7d6b-44c4-809d-872167d1940c") });

            migrationBuilder.InsertData(
                table: "MyBaseEntity",
                columns: new[] { "Id", "BackgroundId", "Discriminator", "EffectId" },
                values: new object[] { new Guid("b43c3275-070c-4685-b496-bedf171c38b8"), new Guid("8c316892-8b7d-47f8-9b96-b4da3418739c"), "MyBackgroundEffectBinding", new Guid("f7eb835e-4c39-4655-9545-61ced8bce356") });

            migrationBuilder.CreateIndex(
                name: "IX_MyBaseEntity_AbilityId",
                table: "MyBaseEntity",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_MyBaseEntity_BackgroundId",
                table: "MyBaseEntity",
                column: "BackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_MyBaseEntity_EffectId",
                table: "MyBaseEntity",
                column: "EffectId");

            migrationBuilder.CreateIndex(
                name: "IX_MyBaseEntity_TraitId",
                table: "MyBaseEntity",
                column: "TraitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyBaseEntity");
        }
    }
}
