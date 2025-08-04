using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularMarketplace.Server.Migrations
{
    /// <inheritdoc />
    public partial class Seller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SellerID",
                table: "tblProducts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblProducts_SellerID",
                table: "tblProducts",
                column: "SellerID");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_AspNetUsers_SellerID",
                table: "tblProducts",
                column: "SellerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_AspNetUsers_SellerID",
                table: "tblProducts");

            migrationBuilder.DropIndex(
                name: "IX_tblProducts_SellerID",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "SellerID",
                table: "tblProducts");
        }
    }
}
