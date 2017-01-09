using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OcenaKlientow.Model.Models
{
    public class Benefit
    {
        [Required]
        [Key]
        public int BenefitId { get; set; }

        [Required]
        public string DataUaktyw { get; set; }

        public string DataZakon { get; set; }

        [Range(0, int.MaxValue)]
        public int LiczbaDni { get; set; }

        [Required]
        public string Nazwa { get; set; }

        public string Opis { get; set; }
        
        [Required]
        public int RodzajId { get; set; }
        
        //public virtual RodzajBenefitu RodzajBenefitu { get; set; }

        [Required]
        public double WartoscProc { get; set; }
    }
}
