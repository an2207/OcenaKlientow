using System.ComponentModel.DataAnnotations;

namespace OcenaKlientow.Model.Models
{
    public class Klient
    {
        #region Properties

        [Required]
        public bool CzyFizyczna { get; set; }

        public string DrugieImie { get; set; }

        public string DrugieNazwisko { get; set; }

        public string Imie { get; set; }

        [Key]
        [Required]
        public int KlientId { get; set; }

        [Range(0.0, double.MaxValue)]
        public double KwotaKredytu { get; set; }

        public string Nazwa { get; set; }

        public string Nazwisko { get; set; }

        //[Unique]
        public string NIP { get; set; }

        //[Unique]
        public string PESEL { get; set; }

        #endregion
    }
}