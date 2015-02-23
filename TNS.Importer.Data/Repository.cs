using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.Interfaces;
using TNS.Importer.ModelInterfaces;

namespace TNS.Importer.Data
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private ImporterDataContext _ctx;

        private DbSet<T> _tmpSet = null;
        private DbSet<T> _set
        {
            get
            {
                var ret = _tmpSet;
                if (ret == null)
                    ret = _ctx.Set<T>();

                return ret;
            }
        }

        public Repository(ImporterDataContext dbContext)
        {
            this._ctx = dbContext;
        }


        public virtual Task<IEnumerable<T>> ListAllAsync()
        {
            return getAllAsync();

        }

        public virtual IEnumerable<T> ListAll()
        {
            return _set;
        }

        async Task<IEnumerable<T>> getAllAsync()
        {
            return await _set.ToListAsync();
        }

        public virtual T Find(int id)
        {
            return _set.Find(id);
        }

        public virtual T Add(T entity)
        {
            _ctx.Entry<T>(entity).State = EntityState.Added;
            return entity;
        }

        public virtual T Update(T entity)
        {
            _ctx.Entry<T>(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual void Delete(T entity)
        {
            _ctx.Entry<T>(entity).State = EntityState.Deleted;
        }


        public virtual void Dispose()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
                _ctx = null;
            }
        }
    }
}
