using BookBazaar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBazaar.DataAccess.Repository.IRepository
{
    internal interface ICategoryRepository : ICategoryRepository<Category>
    {

        void Update(Category obj);
        void Save();
    }
}
