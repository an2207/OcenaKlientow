using SQLite.Net.Attributes;

namespace OcenaKlientowApp1.Model.Models
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