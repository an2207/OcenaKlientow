using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OcenaKlientow.Model.Models
{
    public class Status
    {
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
    }
}
