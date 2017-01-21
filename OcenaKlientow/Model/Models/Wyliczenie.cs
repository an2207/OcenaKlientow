using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OcenaKlientow.Model.Models
{
    public class Wyliczenie
    {
        #region Properties

        [Key]
        [Column(Order = 0)]
        [Required]
        public int OcenaId { get; set; }

        //public virtual Ocena Ocena { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required]
        public int ParametrId { get; set; }

        //public virtual Parametr Parametr { get; set; }

        [Required]
        public int WartoscWyliczona { get; set; }

        #endregion
    }
}