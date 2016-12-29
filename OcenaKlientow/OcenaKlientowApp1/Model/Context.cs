using Microsoft.EntityFrameworkCore;
using OcenaKlientowApp1.Model.Models;

namespace OcenaKlientowApp1.Model
{
    class Context : DbContext
    {
        public DbSet<Benefit> Benefity { get; set; }
        public DbSet<Klient> Klienci { get; set; }
        public DbSet<Ocena> Oceny { get; set; }
        public DbSet<Parametr> Parametry { get; set; }
        public DbSet<Platnosc> Platnosci { get; set; }
        public DbSet<PrzypisanyStatus> PrzypisaneStatusy { get; set; }
        public DbSet<RodzajBenefitu> RodzajeBenefitow { get; set; }
        public DbSet<Status> Statusy { get; set; }
        public DbSet<Wyliczenie> Wyliczono { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=OcenaKlientow.db");
        }
    }
}
