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
           var objFromDb = _db.products.FirstOrDefault(u=>u.Id == obj.Id);  
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Description = obj.Description;    
                objFromDb.Price = obj.Price;    
                objFromDb.Price50 = obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                objFromDb.Price100 = obj.Price100;

                if(obj.ImgUrl != null)
                {
                    objFromDb.ImgUrl = obj.ImgUrl;  
                }
            }
        }
    }
}
