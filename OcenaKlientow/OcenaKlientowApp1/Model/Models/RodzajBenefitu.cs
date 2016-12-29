using SQLite.Net.Attributes;

namespace OcenaKlientowApp1.Model.Models
{
    public class RodzajBenefitu
    {
        #region Properties

        [NotNull]
        [Unique]
        public string Nazwa { get; set; }

        [NotNull]
        [PrimaryKey]
        public int RodzajId { get; set; }

        #endregion
    }
}