using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OcenaKlientow.Migrations
{
    public partial class temp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Benefity",
                columns: table => new
                {
                    BenefitId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataUaktyw = table.Column<string>(nullable: true),
                    DataZakon = table.Column<string>(nullable: true),
                    LiczbaDni = table.Column<int>(nullable: false),
                    Nazwa = table.Column<string>(nullable: true),
                    Opis = table.Column<string>(nullable: true),
                    RodzajId = table.Column<int>(nullable: false),
                    WartoscProc = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefity", x => x.BenefitId);
                });

            migrationBuilder.CreateTable(
                name: "Klienci",
                columns: table => new
                {
                    KlientId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CzyFizyczna = table.Column<bool>(nullable: false),
                    DrugieImie = table.Column<string>(nullable: true),
                    DrugieNazwisko = table.Column<string>(nullable: true),
                    Imie = table.Column<string>(nullable: true),
                    KwotaKredytu = table.Column<double>(nullable: false),
                    NIP = table.Column<string>(nullable: true),
                    Nazwa = table.Column<string>(nullable: true),
                    Nazwisko = table.Column<string>(nullable: true),
                    PESEL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klienci", x => x.KlientId);
                });

            migrationBuilder.CreateTable(
                name: "Oceny",
                columns: table => new
                {
                    OcenaId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataCzas = table.Column<string>(nullable: true),
                    KlientId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    SumaPkt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oceny", x => x.OcenaId);
                });

            migrationBuilder.CreateTable(
                name: "Parametry",
                columns: table => new
                {
                    ParametrId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(nullable: true),
                    Wartosc = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parametry", x => x.ParametrId);
                });

            migrationBuilder.CreateTable(
                name: "Platnosci",
                columns: table => new
                {
                    PlatnoscId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataWymag = table.Column<string>(nullable: true),
                    DataZaplaty = table.Column<string>(nullable: true),
                    Kwota = table.Column<double>(nullable: false),
                    ZamowienieId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platnosci", x => x.PlatnoscId);
                });

            migrationBuilder.CreateTable(
                name: "PrzypisaneStatusy",
                columns: table => new
                {
                    BenefitId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrzypisaneStatusy", x => new { x.BenefitId, x.StatusId });
                });

            migrationBuilder.CreateTable(
                name: "RodzajeBenefitow",
                columns: table => new
                {
                    RodzajId = table.Column<int>(nullable: false),
                    Nazwa = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RodzajeBenefitow", x => new { x.RodzajId, x.Nazwa });
                });

            migrationBuilder.CreateTable(
                name: "Statusy",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(nullable: true),
                    ProgDolny = table.Column<int>(nullable: false),
                    ProgGorny = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statusy", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Wyliczono",
                columns: table => new
                {
                    OcenaId = table.Column<int>(nullable: false),
                    ParametrId = table.Column<int>(nullable: false),
                    WartoscWyliczona = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wyliczono", x => new { x.OcenaId, x.ParametrId });
                });

            migrationBuilder.CreateTable(
                name: "Zamowienia",
                columns: table => new
                {
                    ZamowienieId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CzyZweryfikowano = table.Column<bool>(nullable: false),
                    DataZamowienia = table.Column<string>(nullable: true),
                    KlientId = table.Column<int>(nullable: false),
                    Kwota = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zamowienia", x => x.ZamowienieId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Benefity");

            migrationBuilder.DropTable(
                name: "Klienci");

            migrationBuilder.DropTable(
                name: "Oceny");

            migrationBuilder.DropTable(
                name: "Parametry");

            migrationBuilder.DropTable(
                name: "Platnosci");

            migrationBuilder.DropTable(
                name: "PrzypisaneStatusy");

            migrationBuilder.DropTable(
                name: "RodzajeBenefitow");

            migrationBuilder.DropTable(
                name: "Statusy");

            migrationBuilder.DropTable(
                name: "Wyliczono");

            migrationBuilder.DropTable(
                name: "Zamowienia");
        }
    }
}
