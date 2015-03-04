using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.ModelInterfaces;


namespace TNS.Importer.Interfaces
{
    public interface IRepository<T> : IDisposable where T : IEntity
    {
        Task<IEnumerable<T>> ListAllAsync();
        IEnumerable<T> ListAll();
        T Find(int id);
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        
    }
}
