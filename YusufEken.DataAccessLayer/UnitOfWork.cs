using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YusufEken.DataAccessLayer.Repositories;

namespace YusufEken.DataAccessLayer
{
    public class UnitOfWork : IDisposable
    {
        private readonly DatabaseContext _context;

        private BolumRepository _bolumRepository;
        private DersRepository _dersRepository;
        private KullaniciRepository _kullaniciRepository;
        private PersonelRepository _personelRepository;
        private DonemRepository _donemRepository;

        public BolumRepository BolumRepository
        {
            get
            {
                if (_bolumRepository == null)
                    _bolumRepository = new BolumRepository(_context);
                return _bolumRepository;
            }
        }

        public DersRepository DersRepository
        {
            get
            {
                if (_dersRepository == null)
                    _dersRepository = new DersRepository(_context);
                return _dersRepository;
            }
        }

        public KullaniciRepository KullaniciRepository
        {
            get
            {
                if (_kullaniciRepository == null)
                    _kullaniciRepository = new KullaniciRepository(_context);
                return _kullaniciRepository;
            }
        }

        public PersonelRepository PersonelRepository
        {
            get
            {
                if (_personelRepository == null)
                    _personelRepository = new PersonelRepository(_context);
                return _personelRepository;
            }
        }

        public DonemRepository DonemRepository
        {
            get
            {
                if (_donemRepository == null)
                    _donemRepository = new DonemRepository(_context);
                return _donemRepository;
            }
        }

        public UnitOfWork()
        {
            _context = new DatabaseContext();
        }

        public void Save()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("hata var beritabanında", ex);
                }
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
