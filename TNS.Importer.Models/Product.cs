using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace TNS.Importer.Models
{
    public class Product
    {
        public Product()
        {
            this.Scores = new List<Score>();
        }
        [Required(AllowEmptyStrings=false)]
        [StringLength(50)]
        public string ProductName { get; set; }
        
        public string Scorer { get; set; }

        [DataType(DataType.EmailAddress)]
        public string ScorerEmail { get; set; }

        public DateTime DateOfScoreInput { get; set; }
        
        public List<Score> Scores { get; set; }
    }

}
