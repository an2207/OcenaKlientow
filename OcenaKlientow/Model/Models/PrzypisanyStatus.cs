using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcenaKlientow.Model.Models
{
    public class PrzypisanyStatus
    {
        [Key, Required]
        public int BenefitId { get; set; }

        //public virtual Benefit Benefit { get; set; }
        
        [Key, Required]
        public int StatusId { get; set; }

        //public virtual Status Status { get; set; }
    }
}
