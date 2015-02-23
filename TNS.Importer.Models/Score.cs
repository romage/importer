using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.ModelInterfaces;

namespace TNS.Importer.Models
{
    public class Score: IEntity
    {
        public int Id { get; set;  }
        public Product Product { get; set; }
        public string ScoreName { get; set; }
        public double ScoreValue { get; set; }
    }
}
