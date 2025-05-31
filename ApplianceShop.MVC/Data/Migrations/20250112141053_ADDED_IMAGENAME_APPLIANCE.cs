using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplianceShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class ADDED_IMAGENAME_APPLIANCE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Appliances",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Appliances");
        }
    }
}
