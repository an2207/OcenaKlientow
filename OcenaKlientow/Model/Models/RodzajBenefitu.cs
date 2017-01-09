using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcenaKlientow.Model.Models
{
    public class RodzajBenefitu
    {
        [Required]
        //unique
        public string Nazwa { get; set; }

        [Required, Key]
        public int RodzajId { get; set; }
    }
}
