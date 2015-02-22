using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.Interfaces;

namespace TNS.Importer.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable 
    {
        private ImporterDataContext _ctx;

        public UnitOfWork(ImporterDataContext dbContext)
        {
            this._ctx = dbContext;
        }

        public void SaveChanges()
        {
            _ctx.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _ctx.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
                _ctx = null;
            }
        }
    }
}
