using System.ComponentModel.DataAnnotations;

namespace OcenaKlientow.Model.Models
{
    public class Parametr
    {
        #region Properties

        [Required]
        public string Nazwa { get; set; }

        [Key]
        [Required]
        public int ParametrId { get; set; }

        [Required]
        public int Wartosc { get; set; }

        #endregion
    }
}