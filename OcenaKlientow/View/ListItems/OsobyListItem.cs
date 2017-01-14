using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OcenaKlientow.Model.Models;

namespace OcenaKlientow.View.ListItems
{
    public class OsobyListItem
    {
        public bool CzyFizyczna { get; set; }

        public string DrugieImie { get; set; }

        public string DrugieNazwisko { get; set; }

        public string Imie { get; set; }
        
        public int KlientId { get; set; }
        
        public double KwotaKredytu { get; set; }

        public string Nazwa { get; set; }

        public string Nazwisko { get; set; }
        
        public string NIP { get; set; }
        
        public string PESEL { get; set; }

        public string DataCzas { get; set; }
       
        public int OcenaId { get; set; }
        
        public int SumaPkt { get; set; }
        
        public string NazwaStatusu { get; set; }
        
        public int ProgDolny { get; set; }
        
        public int ProgGorny { get; set; }

        public int StatusId { get; set; }
    }
}
