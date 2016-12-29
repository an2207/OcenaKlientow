using SQLite.Net.Attributes;

namespace OcenaKlientow.Model.Models
{
    public class Zamowienie
    {
        #region Properties

        public bool CzyZweryfikowano { get; set; }

        [NotNull]
        public string DataZamowienia { get; set; }

        [NotNull]
        public int KlientId { get; set; }

        [NotNull]
        //dodac min
        public double Kwota { get; set; }

        [PrimaryKey]
        [NotNull]
        public int ZamowienieId { get; set; }

        #endregion
    }
}