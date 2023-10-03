namespace RealEstateUser.Core.Domain.RepositoryContracts
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IPropertyRepository PropertyRepository { get; }
        IImageRepository ImageRepository { get; }
        IWishListRepository WishListRepository { get; }
        IPropertyadditionalInfoRepository PropertyadditionalInfoRepository { get; }
        IEnquiryRepository EnquiryRepository { get; }
        int Complete();
        Task<bool> SaveAsync();
    }
}
