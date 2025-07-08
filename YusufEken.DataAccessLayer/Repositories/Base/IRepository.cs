using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YusufEken.DataAccessLayer.Repositories.Base
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T GetItem(int id);
        T Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
