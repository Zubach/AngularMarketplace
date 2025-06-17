using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularMarketplace.Server.Migrations
{
    /// <inheritdoc />
    public partial class ProductCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "tblProducts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblProductCategories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsSubCategory = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProductCategories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tblProductCategories_tblProductCategories_ParentID",
                        column: x => x.ParentID,
                        principalTable: "tblProductCategories",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblProducts_CategoryID",
                table: "tblProducts",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_tblProductCategories_ParentID",
                table: "tblProductCategories",
                column: "ParentID");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblProductCategories_CategoryID",
                table: "tblProducts",
                column: "CategoryID",
                principalTable: "tblProductCategories",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblProductCategories_CategoryID",
                table: "tblProducts");

            migrationBuilder.DropTable(
                name: "tblProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_tblProducts_CategoryID",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "tblProducts");
        }
    }
}
