using System.ComponentModel.DataAnnotations;
using SQLite.Net.Attributes;

namespace OcenaKlientow.Model.Models
{
    public class Benefit
    {
        #region Properties

        [NotNull]
        [PrimaryKey]
        public int BenefitId { get; set; }

        [NotNull]
        public string DataUaktyw { get; set; }

        public string DataZakon { get; set; }

        [Range(0, int.MaxValue)]
        public int LiczbaDni { get; set; }

        [NotNull]
        public string Nazwa { get; set; }

        public string Opis { get; set; }

        [NotNull]
        public int RodzajId { get; set; }

        [NotNull]
        public double WartoscProc { get; set; }

        #endregion
    }
}