using SQLite.Net.Attributes;

namespace OcenaKlientow.Model.Models
{
    public class Status
    {
        #region Properties

        [NotNull]
        [Unique]
        public string Nazwa { get; set; }

        [NotNull]
        public int ProgDolny { get; set; }

        [NotNull]
        public int ProgGorny { get; set; }

        [PrimaryKey]
        [NotNull]
        public int StatusId { get; set; }

        #endregion
    }
}