using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcenaKlientow.Model.Models
{
    public class Platnosc
    {
        [Required]
        public string DataWymag { get; set; }

        public string DataZaplaty { get; set; }

        [Required]
        public double Kwota { get; set; }
        
        [Key]
        [Required]
        public int PlatnoscId { get; set; }

        [Required]
        public int ZamowienieId { get; set; }

        //public virtual Zamowienie Zamowienie { get; set; }
    }
}
