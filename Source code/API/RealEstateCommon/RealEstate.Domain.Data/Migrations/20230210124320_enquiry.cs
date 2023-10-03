using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class enquiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "TourType",
                table: "UserEnquries",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TourType",
                table: "UserEnquries");
        }
    }
}
