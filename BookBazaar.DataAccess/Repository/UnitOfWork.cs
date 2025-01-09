using BookBazaar.Data;
using BookBazaar.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBazaar.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        // Property to provide access to the CategoryRepository
        // It allows other parts of the application to use the CategoryRepository through the
        // UnitOfWork
        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }

        // The private database context that will be shared across repositories
        private readonly ApplicationDbContext _db;
        // Constructor to initialize the UnitOfWork
        // Takes an ApplicationDbContext instance as a parameter and assigns it to _db
        // Also initializes the CategoryRepository with the shared database context
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);//_db is passed as parameter
                                                   //such that CategoryRepository can interact database
                                                //now category property of unitofwork can be used anywhere in the application
            Product = new ProductRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
