using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;
using OcenaKlientow.View.ListItems;

namespace OcenaKlientow.ViewModel
{
    public class KlientViewModel
    {
        #region Private fields

        private CultureInfo culture = new CultureInfo("pt-BR");

        #endregion
        #region Properties

        public bool Saving { get; set; }

        #endregion
        #region Ctors

        public KlientViewModel(bool saving)
        {
            Saving = saving;
        }

        #endregion
        #region Public methods

        public List<KlientView> OsobyFizyczneListQuery()
        {
            using (var db = new OcenaKlientowContext())
            {
                IQueryable<KlientView> statusyQuery =
                    from klient in db.Klienci
                    where klient.CzyFizyczna
                    join ocena in db.Oceny on klient.KlientId equals ocena.KlientId
                    join status in db.Statusy on ocena.StatusId equals status.StatusId
                    select new KlientView
                    {
                        Nazwisko = klient.Nazwisko,
                        Imie = klient.Imie,
                        DrugieNazwisko = klient.DrugieNazwisko,
                        DrugieImie = klient.DrugieImie,
                        CzyFizyczna = klient.CzyFizyczna,
                        KlientId = klient.KlientId,
                        KwotaKredytu = klient.KwotaKredytu,
                        PESEL = klient.PESEL,
                        OcenaId = ocena.OcenaId,
                        DataCzas = ocena.DataCzas,
                        SumaPkt = ocena.SumaPkt,
                        ProgDolny = status.ProgDolny,
                        ProgGorny = status.ProgGorny,
                        NazwaStatusu = status.Nazwa
                    };
                List<KlientView> lista = statusyQuery.ToList();
                List<KlientView> secList = lista;
                List<KlientView> filtered = statusyQuery.ToList();
                foreach (KlientView osobyListItem in filtered)
                    foreach (KlientView listItem in filtered)
                        if ((listItem.KlientId == osobyListItem.KlientId) && (listItem.OcenaId < osobyListItem.OcenaId))
                            secList = secList.Where(item => item.OcenaId != listItem.OcenaId).ToList();
                return secList.OrderBy(item => item.KlientId).ToList();
            }
        }

        public List<KlientView> OsobyPrawneListQuery()
        {
            using (var db = new OcenaKlientowContext())
            {
                IQueryable<KlientView> statusyQuery =
                    from klient in db.Klienci
                    where !klient.CzyFizyczna
                    join ocena in db.Oceny on klient.KlientId equals ocena.KlientId
                    join status in db.Statusy on ocena.StatusId equals status.StatusId
                    select new KlientView
                    {
                        Nazwa = klient.Nazwa,
                        CzyFizyczna = klient.CzyFizyczna,
                        KlientId = klient.KlientId,
                        KwotaKredytu = klient.KwotaKredytu,
                        NIP = klient.NIP,
                        OcenaId = ocena.OcenaId,
                        DataCzas = ocena.DataCzas,
                        SumaPkt = ocena.SumaPkt,
                        NazwaStatusu = status.Nazwa,
                        StatusId = status.StatusId,
                        ProgDolny = status.ProgDolny,
                        ProgGorny = status.ProgGorny
                    };
                List<KlientView> lista = statusyQuery.ToList();
                List<KlientView> secList = lista;
                List<KlientView> filtered = statusyQuery.ToList();
                foreach (KlientView osobyListItem in filtered)
                    foreach (KlientView listItem in filtered)
                        if ((listItem.KlientId == osobyListItem.KlientId) && (listItem.OcenaId < osobyListItem.OcenaId))
                            secList = secList.Where(item => item.OcenaId != listItem.OcenaId).ToList();
                return secList.OrderBy(item => item.KlientId).ToList();
            }
        }

        public Klient ReadKlient(int klientId)
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.Klienci.Where(klient => klient.KlientId == klientId).FirstOrDefault();
            }
        }

        #endregion
    }
}