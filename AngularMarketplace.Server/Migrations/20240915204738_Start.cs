using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularMarketplace.Server.Migrations
{
    /// <inheritdoc />
    public partial class Start : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblProducts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(type: "nvarhcar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    img1 = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    img2 = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    img3 = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    img4 = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    img5 = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    img6 = table.Column<string>(type: "nvarchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProducts", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblProducts");
        }
    }
}
