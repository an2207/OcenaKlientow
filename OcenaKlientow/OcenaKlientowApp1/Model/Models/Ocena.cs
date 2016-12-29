using SQLite.Net.Attributes;

namespace OcenaKlientowApp1.Model.Models

{
    public class Ocena
    {
        #region Properties

        [NotNull]
        public string DataCzas { get; set; }

        [NotNull]
        public int KlientId { get; set; }

        [NotNull]
        [PrimaryKey]
        public int OcenaId { get; set; }

        [NotNull]
        public int StatusId { get; set; }

        [NotNull]
        public int SumaPkt { get; set; }

        #endregion
    }
}