using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularMarketplace.Server.Migrations
{
    /// <inheritdoc />
    public partial class Producernewfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Producers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producers_CreatorId",
                table: "Producers",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Producers_AspNetUsers_CreatorId",
                table: "Producers",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producers_AspNetUsers_CreatorId",
                table: "Producers");

            migrationBuilder.DropIndex(
                name: "IX_Producers_CreatorId",
                table: "Producers");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Producers");


        }
    }
}
