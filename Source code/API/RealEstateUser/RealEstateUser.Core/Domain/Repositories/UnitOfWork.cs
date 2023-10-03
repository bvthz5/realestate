using RealEstate.Domain.Data.Data;
using RealEstateUser.Core.Domain.RepositoryContracts;

namespace RealEstateUser.Core.Domain.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RealEstateDbContext _context;
        private bool _disposed = false;
        public UnitOfWork(RealEstateDbContext context)
        {
            _context = context;

            UserRepository = new UserRepository(_context);
            CategoryRepository = new CategoryRepository(_context);
            PropertyRepository = new PropertyRepository(_context);
            ImageRepository = new ImageRepository(_context);
            WishListRepository = new WishListRepository(_context);
            PropertyadditionalInfoRepository = new PropertyadditionalInfoRepository(_context);
            EnquiryRepository = new EnquiryRepository(_context);
        }
        public IUserRepository UserRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IPropertyRepository PropertyRepository { get; private set; }
        public IImageRepository ImageRepository { get; private set; }
        public IWishListRepository WishListRepository { get; private set; }
        public IPropertyadditionalInfoRepository PropertyadditionalInfoRepository { get; private set; }
        public IEnquiryRepository EnquiryRepository { get; private set; }


        public virtual int Complete()
        {
            return _context.SaveChanges();
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
