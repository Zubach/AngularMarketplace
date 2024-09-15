using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularMarketplace.Server.Migrations
{
    /// <inheritdoc />
    public partial class Addpricetoproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "price",
                table: "tblProducts",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "tblProducts");
        }
    }
}
