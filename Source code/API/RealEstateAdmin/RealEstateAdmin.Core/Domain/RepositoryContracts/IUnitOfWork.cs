namespace RealEstateAdmin.Core.Domain.RepositoryContracts
{
    public interface IUnitOfWork : IDisposable
    {
        IAdminRepository AdminRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IEnquiryRepository EnquiryRepository { get; }
        IPropertyRepository PropertyRepository { get; }
        IImageRepository ImageRepository { get; }
        IPropertyAdditionalInfoRepository PropertyAdditionalInfoRepository { get; }
        IUserRepository UserRepository { get; }
        IPropertyUnitRepository PropertyUnitRepository { get; }

        Task<bool> SaveAsync();
        int Complete();
    }
}
