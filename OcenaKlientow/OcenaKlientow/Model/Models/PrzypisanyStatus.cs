using SQLite.Net.Attributes;

namespace OcenaKlientow.Model.Models
{
    public class PrzypisanyStatus
    {
        #region Properties

        [PrimaryKey]
        [NotNull]
        public int BenefitId { get; set; }

        [PrimaryKey]
        [NotNull]
        public int StatusId { get; set; }

        #endregion
    }
}