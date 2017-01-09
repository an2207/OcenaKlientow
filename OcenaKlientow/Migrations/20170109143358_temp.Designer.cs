using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using OcenaKlientow.Model;

namespace OcenaKlientow.Migrations
{
    [DbContext(typeof(OcenaKlientowContext))]
    [Migration("20170109143358_temp")]
    partial class temp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("OcenaKlientow.Model.Models.Benefit", b =>
                {
                    b.Property<int>("BenefitId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DataUaktyw");

                    b.Property<string>("DataZakon");

                    b.Property<int>("LiczbaDni");

                    b.Property<string>("Nazwa");

                    b.Property<string>("Opis");

                    b.Property<int>("RodzajId");

                    b.Property<double>("WartoscProc");

                    b.HasKey("BenefitId");

                    b.ToTable("Benefity");
                });

            modelBuilder.Entity("OcenaKlientow.Model.Models.Klient", b =>
                {
                    b.Property<int>("KlientId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CzyFizyczna");

                    b.Property<string>("DrugieImie");

                    b.Property<string>("DrugieNazwisko");

                    b.Property<string>("Imie");

                    b.Property<double>("KwotaKredytu");

                    b.Property<string>("NIP");

                    b.Property<string>("Nazwa");

                    b.Property<string>("Nazwisko");

                    b.Property<string>("PESEL");

                    b.HasKey("KlientId");

                    b.ToTable("Klienci");
                });

            modelBuilder.Entity("OcenaKlientow.Model.Models.Ocena", b =>
                {
                    b.Property<int>("OcenaId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DataCzas");

                    b.Property<int>("KlientId");

                    b.Property<int>("StatusId");

                    b.Property<int>("SumaPkt");

                    b.HasKey("OcenaId");

                    b.ToTable("Oceny");
                });

            modelBuilder.Entity("OcenaKlientow.Model.Models.Parametr", b =>
                {
                    b.Property<int>("ParametrId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nazwa");

                    b.Property<int>("Wartosc");

                    b.HasKey("ParametrId");

                    b.ToTable("Parametry");
                });

            modelBuilder.Entity("OcenaKlientow.Model.Models.Platnosc", b =>
                {
                    b.Property<int>("PlatnoscId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DataWymag");

                    b.Property<string>("DataZaplaty");

                    b.Property<double>("Kwota");

                    b.Property<int>("ZamowienieId");

                    b.HasKey("PlatnoscId");

                    b.ToTable("Platnosci");
                });

            modelBuilder.Entity("OcenaKlientow.Model.Models.PrzypisanyStatus", b =>
                {
                    b.Property<int>("BenefitId");

                    b.Property<int>("StatusId");

                    b.HasKey("BenefitId", "StatusId");

                    b.ToTable("PrzypisaneStatusy");
                });

            modelBuilder.Entity("OcenaKlientow.Model.Models.RodzajBenefitu", b =>
                {
                    b.Property<int>("RodzajId");

                    b.Property<string>("Nazwa");

                    b.HasKey("RodzajId", "Nazwa");

                    b.ToTable("RodzajeBenefitow");
                });

            modelBuilder.Entity("OcenaKlientow.Model.Models.Status", b =>
                {
                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nazwa");

                    b.Property<int>("ProgDolny");

                    b.Property<int>("ProgGorny");

                    b.HasKey("StatusId");

                    b.ToTable("Statusy");
                });

            modelBuilder.Entity("OcenaKlientow.Model.Models.Wyliczenie", b =>
                {
                    b.Property<int>("OcenaId");

                    b.Property<int>("ParametrId");

                    b.Property<int>("WartoscWyliczona");

                    b.HasKey("OcenaId", "ParametrId");

                    b.ToTable("Wyliczono");
                });

            modelBuilder.Entity("OcenaKlientow.Model.Models.Zamowienie", b =>
                {
                    b.Property<int>("ZamowienieId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CzyZweryfikowano");

                    b.Property<string>("DataZamowienia");

                    b.Property<int>("KlientId");

                    b.Property<double>("Kwota");

                    b.HasKey("ZamowienieId");

                    b.ToTable("Zamowienia");
                });
        }
    }
}
