using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookBazaar.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // T = Category, product or order or so on anything where we want to perform crud operation
        // or interact with DbContext
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter);//FirstOrDefault(u => u.Id == id)
                                                //to write this linq operation we use this general syntax
        void Add(T entity); //object which needs to be added.
      //  void Update(T entity);
        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity); //collection of entity
    }
}
