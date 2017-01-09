using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcenaKlientow.Model.Models
{
    public class Ocena
    {
        [Required]
        public string DataCzas { get; set; }

        [Required]
        public int KlientId { get; set; }

        //public virtual Klient Klient { get; set; }

        [Required]
        [Key]
        public int OcenaId { get; set; }

        [Required]
        public int StatusId { get; set; }

        //public virtual Status Status { get; set; }

        [Required]
        public int SumaPkt { get; set; }
    }
}
