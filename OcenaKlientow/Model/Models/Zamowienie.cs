using System.ComponentModel.DataAnnotations;

namespace OcenaKlientow.Model.Models
{
    public class Zamowienie
    {
        #region Properties

        public bool CzyZweryfikowano { get; set; }

        [Required]
        public string DataZamowienia { get; set; }

        [Required]
        public int KlientId { get; set; }

        //public virtual Klient Klient { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        public double Kwota { get; set; }

        [Key]
        [Required]
        public int ZamowienieId { get; set; }

        #endregion
    }
}