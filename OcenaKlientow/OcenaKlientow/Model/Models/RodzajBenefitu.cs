using SQLite.Net.Attributes;

namespace OcenaKlientow.Model.Models
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