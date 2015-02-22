using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS.Importer.Data
{
    public class ImporterDataContext: DbContext
    {
        public ImporterDataContext()
            : base("DefaultConnection")
        {}



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
