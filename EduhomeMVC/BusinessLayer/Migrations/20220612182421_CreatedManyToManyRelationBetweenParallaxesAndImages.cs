using Microsoft.EntityFrameworkCore.Migrations;

namespace BusinessLayer.Migrations
{
    public partial class CreatedManyToManyRelationBetweenParallaxesAndImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parallaxes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parallaxes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParallaxImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParallaxId = table.Column<int>(nullable: false),
                    ImageId = table.Column<int>(nullable: false),
                    IsMain = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParallaxImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParallaxImages_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParallaxImages_Parallaxes_ParallaxId",
                        column: x => x.ParallaxId,
                        principalTable: "Parallaxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParallaxImages_ImageId",
                table: "ParallaxImages",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ParallaxImages_ParallaxId",
                table: "ParallaxImages",
                column: "ParallaxId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParallaxImages");

            migrationBuilder.DropTable(
                name: "Parallaxes");
        }
    }
}
