using System.ComponentModel.DataAnnotations;

namespace OcenaKlientow.Model.Models
{
    public class Platnosc
    {
        #region Properties

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

        #endregion

        //public virtual Zamowienie Zamowienie { get; set; }
    }
}