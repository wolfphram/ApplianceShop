using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplianceShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fixed_AppPartsDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Parts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Parts");
        }
    }
}
