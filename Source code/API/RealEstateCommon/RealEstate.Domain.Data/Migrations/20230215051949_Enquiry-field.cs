using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class Enquiryfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TourType",
                table: "UserEnquries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "TourType",
                table: "UserEnquries",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
