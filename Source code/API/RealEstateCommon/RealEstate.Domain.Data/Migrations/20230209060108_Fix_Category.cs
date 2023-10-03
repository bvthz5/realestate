using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


            migrationBuilder.Sql(
                $"INSERT INTO Categories (CategoryName, Type, Status, IsActive, IsDeleted, CreatedOn, UpdatedOn, CreatedBy)" +
                $" VALUES('Rental Buildings',1, 1, 'True', 'False','{date}', '{date}', '0' )");


            migrationBuilder.Sql(
                $"INSERT INTO Categories (CategoryName, Type, Status, IsActive, IsDeleted, CreatedOn, UpdatedOn, CreatedBy)" +
                $" VALUES('Apartments For Rent',1, 1, 'True', 'False','{date}', '{date}', '0' )");


            migrationBuilder.Sql(
                $"INSERT INTO Categories (CategoryName, Type, Status, IsActive, IsDeleted, CreatedOn, UpdatedOn, CreatedBy)" +
                $" VALUES('Houses For Rent',1, 1, 'True', 'False','{date}', '{date}', '0' )");


            migrationBuilder.Sql(
                $"INSERT INTO Categories (CategoryName, Type, Status, IsActive, IsDeleted, CreatedOn, UpdatedOn, CreatedBy )" +
                $" VALUES('Homes For Sale',2, 1, 'True', 'False','{date}', '{date}', '0')");


            migrationBuilder.Sql(
                $"INSERT INTO Categories (CategoryName, Type, Status, IsActive, IsDeleted, CreatedOn, UpdatedOn, CreatedBy)" +
                $" VALUES('Open Houses',2, 1, 'True', 'False','{date}', '{date}', '0' )");


            migrationBuilder.Sql(
                $"INSERT INTO Categories (CategoryName, Type, Status, IsActive, IsDeleted, CreatedOn, UpdatedOn, CreatedBy)" +
                $" VALUES('Plot',2, 1, 'True', 'False','{date}', '{date}', '0')");


        }
    }
}
