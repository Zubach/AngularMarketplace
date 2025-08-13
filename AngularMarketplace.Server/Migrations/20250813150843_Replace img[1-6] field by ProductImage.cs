using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularMarketplace.Server.Migrations
{
    /// <inheritdoc />
    public partial class Replaceimg16fieldbyProductImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "img1",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "img2",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "img3",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "img4",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "img5",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "img6",
                table: "tblProducts");

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductImage_tblProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tblProducts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.AddColumn<string>(
                name: "img1",
                table: "tblProducts",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "img2",
                table: "tblProducts",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "img3",
                table: "tblProducts",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "img4",
                table: "tblProducts",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "img5",
                table: "tblProducts",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "img6",
                table: "tblProducts",
                type: "nvarchar(30)",
                nullable: true);
        }
    }
}
