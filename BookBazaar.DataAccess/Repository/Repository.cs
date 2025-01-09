using BookBazaar.Data;
using BookBazaar.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookBazaar.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        //we cant simply use _db.categories.add because we dont know which class the t represents.

        internal DbSet<T> dbset;//use generic to access Dbset directly
        //dbSet = it represent table for entity type t

        public Repository(ApplicationDbContext db)//applicationdbcontext will be provided to when the
                                                  //object will be create (look CategoryRepository.cs)
        {
            _db = db;
            this.dbset = _db.Set<T>(); 
            //_db.categories = dbset
            
        }
        public void Add(T entity)
        {
            // _db.Categories.Add(T); this is not valid as we dont know which
            // specify entity we are working with can be category, product and so on
            dbset.Add(entity);//_db.Categories.Add(T); same
        }
        

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);// filter is condition
                                        // _db.categories.Where(u=>u.Id == id).FirstOrDefault();
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbset;
            return query.ToList();  
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }
    }
}
