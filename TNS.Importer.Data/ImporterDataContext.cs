using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.Models;

namespace TNS.Importer.Data
{
    public class ImporterDataContext: DbContext
    {
        public ImporterDataContext()
            : base("DefaultConnection")
        {}

        public DbSet<Product> Products { get; set; }
        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
