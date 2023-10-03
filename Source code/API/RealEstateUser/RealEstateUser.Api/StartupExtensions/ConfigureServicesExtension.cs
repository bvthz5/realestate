using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstateUser.Core.Domain.Repositories;
using RealEstateUser.Core.Domain.RepositoryContracts;
using RealEstateUser.Core.Security;
using RealEstateUser.Core.Security.Util;
using RealEstateUser.Core.ServiceContracts;
using RealEstateUser.Core.Services;
using RealEstateUser.Core.Settings;

namespace RealEstateUser.Api.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
        {

            //db configuration => 
            services.AddDbContext<RealEstateDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
                ));

            //repository =>
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));



            // services =>
            services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            services.AddScoped(typeof(IEmailService), typeof(EmailService));
            services.AddScoped(typeof(IEnquiryService), typeof(EnquiryService));
            services.AddScoped(typeof(IGoogleService), typeof(GoogleService));
            services.AddScoped(typeof(IImageService), typeof(ImageService));
            services.AddScoped(typeof(IPropertyService), typeof(PropertyService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IWishListService), typeof(WishListService));

            // security =>
            services.AddScoped(typeof(GoogleSecurity));
            services.AddScoped(typeof(TokenGenerator));
            services.AddScoped(typeof(ImageFileUtil));
            services.AddScoped(typeof(SecurityUtil));

            // settings =>
            services.Configure<GoogleSettings>(configuration.GetSection("GoogleSettings"));
            services.Configure<ImageSettings>(configuration.GetSection("ImageSettings"));
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            return services;

        }
    }
}
