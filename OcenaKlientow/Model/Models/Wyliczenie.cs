using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcenaKlientow.Model.Models
{
    public class Wyliczenie
    {
        [Key]
        [Required]
        public int OcenaId { get; set; }

        //public virtual Ocena Ocena { get; set; }

        [Key]
        [Required]
        public int ParametrId { get; set; }

        //public virtual Parametr Parametr { get; set; }

        [Required]
        public int WartoscWyliczona { get; set; }
    }
}
