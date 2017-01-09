using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcenaKlientow.Model.Models
{
    public class Parametr
    {
        [Required]
        public string Nazwa { get; set; }

        [Key]
        [Required]
        public int ParametrId { get; set; }

        [Required]
        public int Wartosc { get; set; }
    }
}
