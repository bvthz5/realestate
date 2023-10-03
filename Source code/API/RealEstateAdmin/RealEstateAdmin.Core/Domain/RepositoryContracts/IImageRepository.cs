using RealEstate.Domain.Data.Entities;

namespace RealEstateAdmin.Core.Domain.RepositoryContracts
{
    public interface IImageRepository : IGenericRepository<Image>
    {
        Task<List<Image>> FindByPropertyIdAsync(int propertyId);
        void DeleteImages(List<Image> images);
        void DeleteImage(Image image);
        Task<Image?> FindById(int ImageId);
    }
}
