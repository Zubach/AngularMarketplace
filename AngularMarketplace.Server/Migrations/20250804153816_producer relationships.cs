using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularMarketplace.Server.Migrations
{
    /// <inheritdoc />
    public partial class producerrelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producer_tblProductCategories_CategoryID",
                table: "Producer");

            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_Producer_ProducerID",
                table: "tblProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Producer",
                table: "Producer");

            migrationBuilder.DropIndex(
                name: "IX_Producer_CategoryID",
                table: "Producer");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Producer");

            migrationBuilder.RenameTable(
                name: "Producer",
                newName: "Producers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Producers",
                table: "Producers",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "ProducerCategory",
                columns: table => new
                {
                    ProducerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerCategory", x => new { x.CategoryId, x.ProducerId });
                    table.ForeignKey(
                        name: "FK_ProducerCategory_Producers_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "Producers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProducerCategory_tblProductCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "tblProductCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProducerCategory_ProducerId",
                table: "ProducerCategory",
                column: "ProducerId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_Producers_ProducerID",
                table: "tblProducts",
                column: "ProducerID",
                principalTable: "Producers",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_Producers_ProducerID",
                table: "tblProducts");

            migrationBuilder.DropTable(
                name: "ProducerCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Producers",
                table: "Producers");

            migrationBuilder.RenameTable(
                name: "Producers",
                newName: "Producer");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Producer",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Producer",
                table: "Producer",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Producer_CategoryID",
                table: "Producer",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Producer_tblProductCategories_CategoryID",
                table: "Producer",
                column: "CategoryID",
                principalTable: "tblProductCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_Producer_ProducerID",
                table: "tblProducts",
                column: "ProducerID",
                principalTable: "Producer",
                principalColumn: "ID");
        }
    }
}
