using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS.Importer.Models
{
    public class Product
    {
        public Product()
        {
            this.Scores = new List<Score>();
        }
        public string ProductName { get; set; }
        public string Scorer { get; set; }
        public string ScorerEmail { get; set; }
        public DateTime DateOfScoreInput { get; set; }
        public List<Score> Scores { get; set; }
    }

}
