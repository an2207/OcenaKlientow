using SQLite.Net.Attributes;

namespace OcenaKlientow.Model.Models
{
    public class Platnosc
    {
        #region Properties

        [NotNull]
        public string DataWymag { get; set; }

        public string DataZaplaty { get; set; }

        [NotNull]
        public double Kwota { get; set; }

        [NotNull]
        [PrimaryKey]
        public int PlatnoscId { get; set; }

        [NotNull]
        public int ZamowienieId { get; set; }

        #endregion
    }
}