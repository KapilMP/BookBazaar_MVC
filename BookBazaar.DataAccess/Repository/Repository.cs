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
            _db.products.Include(u => u.Category).Include(u => u.CategoryId);
            //we can ad multiple Include also, it will use to get Category property base on foreign key relationship
            
        }
        public void Add(T entity)
        {
            // _db.Categories.Add(T); this is not valid as we dont know which
            // specify entity we are working with can be category, product and so on
            dbset.Add(entity);//_db.Categories.Add(T); same
        }
        

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbset;
            }
            else
            {
                query = dbset.AsNoTracking();
            }
            query = query.Where(filter);// filter is condition
                                        // _db.categories.Where(u=>u.Id == id).FirstOrDefault();

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.
                    Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        //Category,CoverLetter
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                //if more include then separate with ","
                foreach(var includeProp in includeProperties.
                    Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
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
