using BookBazaar.Data;
using BookBazaar.DataAccess.Repository.IRepository;
using BookBazaar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBazaar.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            _db.products.Update(obj);
        }
    }
}
