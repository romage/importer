using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TNS.Importer.ModelInterfaces;

namespace TNS.Importer.Models
{
    public class Product: IEntity
    {
        public Product()
        {
            this.Scores = new List<Score>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings=false)]
        [StringLength(50)]
        public string ProductName { get; set; }

        public string OriginalFileName { get; set; }
        public string SystemFileNameWithExtension { get; set; }
        public string CurrentProcessingFolder { get; set; }

        public ProcessStateEnum ProcessState { get; set; }
        
        public string Scorer { get; set; }

        [DataType(DataType.EmailAddress)]
        public string ScorerEmail { get; set; }

        public DateTime DateOfScoreInput { get; set; }
        
        public ICollection<Score> Scores { get; set; }
    }

}
