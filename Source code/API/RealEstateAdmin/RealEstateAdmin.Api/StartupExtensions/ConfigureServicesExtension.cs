using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstateAdmin.Core.Domain.Repositories;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.Security;
using RealEstateAdmin.Core.Security.Util;
using RealEstateAdmin.Core.ServiceContracts;
using RealEstateAdmin.Core.Services;
using RealEstateAdmin.Core.Settings;

namespace RealEstateAdmin.Api.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
        {

            //db configuration - 
            services.AddDbContext<RealEstateDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
                ));

            //repository -
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Security
            services.AddScoped(typeof(TokenGenerator));
            services.AddScoped(typeof(SecurityUtil));
            services.AddScoped(typeof(FileUtil));

            // services
            services.AddScoped(typeof(IAdminService), typeof(AdminService));
            services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            services.AddScoped(typeof(IPropertyService), typeof(PropertyService));
            services.AddScoped(typeof(IImageService), typeof(ImageService));
            services.AddScoped(typeof(IPropertyAdditionalInfoService), typeof(PropertyAdditionalInfoService));
            services.AddScoped(typeof(IEmailService), typeof(EmailService));
            services.AddScoped(typeof(IEnquiryService), typeof(EnquiryService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IPropertyUnitService), typeof(PropertyUnitService));






            // Settings
            services.Configure<AdminCredentials>(configuration.GetSection("AdminCredentials"));
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<ImageSettings>(configuration.GetSection("ImageSettings"));

            return services;

        }
    }
}
