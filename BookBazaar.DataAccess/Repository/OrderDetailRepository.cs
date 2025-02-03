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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
            //db context we get we pass to Repository.cs (//here)
        {
            _db = db;

        }

        public void Save()
        {
           _db.SaveChanges();
        }

        public void Update(OrderDetail obj)
        {
            _db.orderDetails.Update(obj);
        }

       
    }
}
