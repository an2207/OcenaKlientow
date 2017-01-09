using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OcenaKlientow.Model.Models
{
    public class Zamowienie
    {
        public bool CzyZweryfikowano { get; set; }

        [Required]
        public string DataZamowienia { get; set; }

        [Required]
        public int KlientId { get; set; }
    
        //public virtual Klient Klient { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue)]
        public double Kwota { get; set; }

        [Key]
        [Required]
        public int ZamowienieId { get; set; }
    }
}
