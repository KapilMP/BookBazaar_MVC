using BookBazaar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBazaar.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        //we dont add update method directly in generic repository because when you are
        //updating category logic might be different compared to product entity
        void Update(OrderDetail obj);
       // void Save();

    }
}
