using System.ComponentModel.DataAnnotations;

namespace OcenaKlientow.Model.Models
{
    public class PrzypisanyStatus
    {
        #region Properties

        [Key]
        [Required]
        public int BenefitId { get; set; }

        //public virtual Benefit Benefit { get; set; }

        [Key]
        [Required]
        public int StatusId { get; set; }

        #endregion

        //public virtual Status Status { get; set; }
    }
}