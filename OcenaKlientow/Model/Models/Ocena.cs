using System.ComponentModel.DataAnnotations;

namespace OcenaKlientow.Model.Models
{
    public class Ocena
    {
        #region Properties

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

        #endregion
    }
}