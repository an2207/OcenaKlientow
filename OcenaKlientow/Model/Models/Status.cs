using System.ComponentModel.DataAnnotations;

namespace OcenaKlientow.Model.Models
{
    public class Status
    {
        #region Properties

        [Required]
        //Unique
        public string Nazwa { get; set; }

        [Required]
        public int ProgDolny { get; set; }

        [Required]
        public int ProgGorny { get; set; }

        [Key]
        [Required]
        public int StatusId { get; set; }

        #endregion
    }
}