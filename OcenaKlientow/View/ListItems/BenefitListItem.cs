using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcenaKlientow.View.ListItems
{
    public class BenefitListItem
    {
        public int BenefitId { get; set; }
        
        public string DataUaktyw { get; set; }

        public string DataZakon { get; set; }
        
        public int LiczbaDni { get; set; }
        
        public string NazwaBenefitu { get; set; }

        public string Opis { get; set; }
        
        public int RodzajId { get; set; }
        
        public double WartoscProc { get; set; }
        
        public string NazwaRodzaju { get; set; }
        
    }
}
