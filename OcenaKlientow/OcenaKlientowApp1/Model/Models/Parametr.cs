using SQLite.Net.Attributes;

namespace OcenaKlientowApp1.Model.Models
{
    public class Parametr
    {
        #region Properties

        [NotNull]
        public string Nazwa { get; set; }

        [PrimaryKey]
        [NotNull]
        public int ParametrId { get; set; }

        [NotNull]
        public int Wartosc { get; set; }

        #endregion
    }
}