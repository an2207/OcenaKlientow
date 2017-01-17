using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OcenaKlientow.Model.Models;

namespace OcenaKlientow.Model
{
    public class OcenaKlientowContext :DbContext
    {
        public DbSet<Platnosc> Platnosci { get; set; }
        public DbSet<Benefit> Benefity { get; set; }
        public DbSet<Klient> Klienci { get; set; }
        public DbSet<Ocena> Oceny { get; set; }
        public DbSet<Parametr> Parametry { get; set; }
        public DbSet<Status> Statusy { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }
        public DbSet<Wyliczenie> Wyliczono { get; set; }
        public DbSet<PrzypisanyStatus> PrzypisaneStatusy { get; set; }
        public DbSet<RodzajBenefitu> RodzajeBenefitow { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite(@"Filename=OcenaKlientow102.1.REF1.0.0.db");
            //optionsBuilder.UseSqlite(@"Filename=OcenaKlientow102.TESTADColumns.db");
            optionsBuilder.UseSqlite(@"Filename=OcenaKlientowInitial.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wyliczenie>().HasKey(wyliczenie => new
            {
                wyliczenie.OcenaId,
                wyliczenie.ParametrId
            });
            modelBuilder.Entity<PrzypisanyStatus>().HasKey(status => new
            {
                status.BenefitId,
                status.StatusId
            });
            modelBuilder.Entity<RodzajBenefitu>().HasKey(benefitu => new
            {
                benefitu.RodzajId,
                benefitu.Nazwa
            });

           
            

        }
    }
}
