using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YusufEken.DataAccessLayer.Repositories.Base;
using YusufEken.EntityLayer.Base;

namespace YusufEken.DataAccessLayer.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        protected readonly DatabaseContext context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DatabaseContext context)
        {
            this.context = context;
            this._dbSet = context.Set<T>();
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public virtual T GetItem(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual T Add(T item)
        {
            return _dbSet.Add(item);
        }

        public virtual void Update(T item)
        {
            if (context.Entry(item).State == EntityState.Detached)
            {
                _dbSet.Attach(item);
            }
            context.Entry(item).State = EntityState.Modified;
        }

        public virtual void Delete(T item)
        {
            if (context.Entry(item).State == EntityState.Detached)
            {
                _dbSet.Attach(item);
            }
            context.Entry(item).State = EntityState.Deleted;
        }
    }
}
