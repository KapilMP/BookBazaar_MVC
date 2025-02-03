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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
            //db context we get we pass to Repository.cs (//here)
        {
            _db = db;

        }

        public void Save()
        {
           _db.SaveChanges();
        }

        public void Update(OrderHeader obj)
        {
            _db.orderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDB = _db.orderHeaders.FirstOrDefault(u => u.Id == id);

            if(orderFromDB != null)
            {
                orderFromDB.OrderStatus = orderStatus;

                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDB.PaymentStatus = paymentStatus;
                }

            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
          var orderFromDb = _db.orderHeaders.FirstOrDefault(u => u.Id==id);
            if(!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SessionId = sessionId;
            }
            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                orderFromDb.PaymentIntentId = paymentIntentId;
                orderFromDb.PaymentDate = DateTime.Now;
            }
        }
    }
}
