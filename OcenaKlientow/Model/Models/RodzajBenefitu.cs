using System.ComponentModel.DataAnnotations;

namespace OcenaKlientow.Model.Models
{
    public class RodzajBenefitu
    {
        #region Properties

        [Required]
        //unique
        public string Nazwa { get; set; }

        [Required]
        [Key]
        public int RodzajId { get; set; }

        #endregion
    }
}