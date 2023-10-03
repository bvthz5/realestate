using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixADMINReg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var name = "Admin";
            var email = "realestate5bv@gmail.com";
            var password = BCrypt.Net.BCrypt.HashPassword("Admin@123");
            var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            migrationBuilder.Sql($"INSERT INTO Admins (Name, Email, Password, CreatedDate, UpdatedDate) VALUES('{name}','{email}','{password}','{date}','{date}')");

        }
    }
}
