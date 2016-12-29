using SQLite.Net.Attributes;

namespace OcenaKlientow.Model.Models
{
    public class Klient
    {
        #region Properties

        [NotNull]
        public bool CzyFizyczna { get; set; }

        public string DrugieImie { get; set; }

        public string DrugieNazwisko { get; set; }

        public string Imie { get; set; }

        [PrimaryKey]
        [NotNull]
        public int KlientId { get; set; }

        //dodac min
        public double KwotaKredytu { get; set; }

        public string Nazwa { get; set; }

        public string Nazwisko { get; set; }

        [Unique]
        public string NIP { get; set; }

        [Unique]
        public string PESEL { get; set; }

        #endregion
    }
}