using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularMarketplace.Server.Migrations
{
    /// <inheritdoc />
    public partial class addimgtoCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "img",
                table: "tblProductCategories",
                type: "nvarchar(30)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "img",
                table: "tblProductCategories");
        }
    }
}
