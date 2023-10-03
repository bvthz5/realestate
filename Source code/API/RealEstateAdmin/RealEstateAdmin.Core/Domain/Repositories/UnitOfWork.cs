using RealEstate.Domain.Data.Data;
using RealEstateAdmin.Core.Domain.RepositoryContracts;

namespace RealEstateAdmin.Core.Domain.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RealEstateDbContext _context;
        private bool _disposed = false;
        public UnitOfWork(RealEstateDbContext context)
        {
            _context = context;
            AdminRepository = new AdminRepository(_context);
            CategoryRepository = new CategoryRepository(_context);
            PropertyRepository = new PropertyRepository(_context);
            ImageRepository = new ImageRepository(_context);
            PropertyAdditionalInfoRepository = new PropertyAdditionalInfoRepository(_context);
            EnquiryRepository = new EnquiryRepository(_context);
            UserRepository = new UserRepository(_context);
            PropertyUnitRepository = new PropertyUnitRepository(_context);

        }
        public IAdminRepository AdminRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IPropertyRepository PropertyRepository { get; private set; }
        public IImageRepository ImageRepository { get; private set; }
        public IPropertyAdditionalInfoRepository PropertyAdditionalInfoRepository { get; private set; }
        public IEnquiryRepository EnquiryRepository { get; private set; }
        public IPropertyUnitRepository PropertyUnitRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
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
