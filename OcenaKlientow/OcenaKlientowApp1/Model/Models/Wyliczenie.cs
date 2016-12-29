using SQLite.Net.Attributes;

namespace OcenaKlientowApp1.Model.Models
{
    public class Wyliczenie
    {
        #region Properties

        [PrimaryKey]
        [NotNull]
        public int OcenaId { get; set; }

        [PrimaryKey]
        [NotNull]
        public int ParametrId { get; set; }

        [NotNull]
        public int WartoscWyliczona { get; set; }

        #endregion
    }
}