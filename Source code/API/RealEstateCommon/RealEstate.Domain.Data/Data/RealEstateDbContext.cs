using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Entities;

namespace RealEstate.Domain.Data.Data
{
    public class RealEstateDbContext : DbContext
    {
        public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<Property> Properties { get; set; } = null!;
        public DbSet<PropertyAdditionalInfo> PropertyInfos { get; set; } = null!;
        public DbSet<PropertyAttachments> PropertyAttachment { get; set; } = null!;
        public DbSet<Enquiry> UserEnquries { get; set; } = null!;
        public DbSet<WishList> WishLists { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<PropertyUnits> PropertyUnits { get; set; } = null!;


    }
}
