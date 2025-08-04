using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularMarketplace.Server.Migrations
{
    /// <inheritdoc />
    public partial class Addproductproducer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProducerID",
                table: "tblProducts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Producer",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Producer_tblProductCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "tblProductCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblProducts_ProducerID",
                table: "tblProducts",
                column: "ProducerID");

            migrationBuilder.CreateIndex(
                name: "IX_Producer_CategoryID",
                table: "Producer",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_Producer_ProducerID",
                table: "tblProducts",
                column: "ProducerID",
                principalTable: "Producer",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_Producer_ProducerID",
                table: "tblProducts");

            migrationBuilder.DropTable(
                name: "Producer");

            migrationBuilder.DropIndex(
                name: "IX_tblProducts_ProducerID",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "ProducerID",
                table: "tblProducts");
        }
    }
}
