using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Tema3Supermarket.Comands;
using Tema3Supermarket.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Tema3Supermarket.ViewModels
{
    public class BonCasaViewModel : BaseViewModel
    {
        private ObservableCollection<Produs> produse;
        public ObservableCollection<Produs> Produse
        {
            get { return produse; }
            set { produse = value; OnPropertyChanged("Produse"); }
        }

        private string criteriuCautare;
        public string CriteriuCautare
        {
            get { return criteriuCautare; }
            set { criteriuCautare = value; OnPropertyChanged("CriteriuCautare"); }
        }

        private Produs produsSelectat;
        public Produs ProdusSelectat
        {
            get { return produsSelectat; }
            set { produsSelectat = value; OnPropertyChanged("ProdusSelectat"); }
        }

        private ObservableCollection<ProdusBon> produseBon;
        public ObservableCollection<ProdusBon> ProduseBon
        {
            get { return produseBon; }
            set { produseBon = value; OnPropertyChanged("ProduseBon"); }
        }

        private decimal totalBon;
        public decimal TotalBon
        {
            get { return totalBon; }
            set { totalBon = value; OnPropertyChanged("TotalBon"); }
        }
        private ObservableCollection<BonCasa> bonuri;
        public ObservableCollection<BonCasa> Bonuri
        {
            get { return bonuri; }
            set { bonuri = value; OnPropertyChanged("Bonuri"); }
        }

        private int numarBonuri;
        public int NumarBonuri
        {
            get { return numarBonuri; }
            set { numarBonuri = value; OnPropertyChanged("NumarBonuri"); }
        }

        private decimal sumaIncasata;
        public decimal SumaIncasata
        {
            get { return sumaIncasata; }
            set { sumaIncasata = value; OnPropertyChanged("SumaIncasata"); }
        }

        public ICommand IncarcaBonuriCommand { get; private set; }
        public ICommand AdaugaProdusPeBonCommand { get; set; }
        public ICommand EmitereBonCommand { get; set; }
        public ICommand CautareProduseCommand { get; set; }

        public BonCasaViewModel()
        {
            Produse = new ObservableCollection<Produs>();
            ProduseBon = new ObservableCollection<ProdusBon>();
            Bonuri = new ObservableCollection<BonCasa>();
            AdaugaProdusPeBonCommand = new RelayCommand(param => AdaugaProdusPeBon());
            EmitereBonCommand = new RelayCommand(param => EmitereBon());
            CautareProduseCommand = new RelayCommand(param => CautareProduse());
            IncarcaBonuriCommand = new RelayCommand(param => IncarcaBonuri());
        }

        private void IncarcaBonuri()
        {
            using (var context = new SupermarketDbContext())
            {
                var bonuriIncarcate = context.BonuriCasa.ToList();
                Bonuri = new ObservableCollection<BonCasa>(bonuriIncarcate);
                NumarBonuri = bonuriIncarcate.Count;
                SumaIncasata = bonuriIncarcate.Sum(b => b.SumaIncasata);
            }
        }

        private void AdaugaProdusPeBon()
        {
            if (ProdusSelectat == null)
            {
                MessageBox.Show("Nu a fost selectat niciun produs.");
                return;
            }

            var stoc = ProdusSelectat.Stocuri.FirstOrDefault();
            if (stoc == null || stoc.Cantitate <= 0)
            {
                MessageBox.Show("Produsul selectat nu are stocuri disponibile.");
                return;
            }

            var produsBon = new ProdusBon
            {
                ProdusId = ProdusSelectat.Id,
                Cantitate = 1,
                Subtotal = stoc.PretVanzare,
                Produs = ProdusSelectat
            };

            var existingProdusBon = ProduseBon.FirstOrDefault(pb => pb.ProdusId == produsBon.ProdusId);
            if (existingProdusBon != null)
            {
                existingProdusBon.Cantitate += 1;
                existingProdusBon.Subtotal += produsBon.Subtotal;
            }
            else
            {
                ProduseBon.Add(produsBon);
            }

            CalculeazaTotal();
        }

        private void CalculeazaTotal()
        {
            TotalBon = ProduseBon.Sum(pb => pb.Subtotal * pb.Cantitate);
        }

        private void EmitereBon()
        {
            try
            {
                using (var context = new SupermarketDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        var bon = new BonCasa
                        {
                            DataEliberare = DateTime.Now,
                            CasierId = 1,  
                            SumaIncasata = TotalBon
                        };

                        context.BonuriCasa.Add(bon);
                        context.SaveChanges();

                        foreach (var produsBon in ProduseBon)
                        {
                            produsBon.BonId = bon.Id;
                            context.ProduseBon.Add(produsBon);

                            var stoc = context.Stocuri.FirstOrDefault(s => s.ProdusId == produsBon.ProdusId);
                            if (stoc != null)
                            {
                                stoc.Cantitate -= produsBon.Cantitate;
                                context.Entry(stoc).State = EntityState.Modified;
                            }
                        }

                        context.SaveChanges();
                        transaction.Commit();
                        MessageBox.Show(FormatBon(), "Bon emis", MessageBoxButton.OK, MessageBoxImage.Information);

                        
                        ProduseBon.Clear();
                        TotalBon = 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = $"Eroare la emiterea bonului: {fullErrorMessage}";
                MessageBox.Show(exceptionMessage);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "N/A";
                MessageBox.Show($"Eroare la emiterea bonului: {ex.Message}\nExcepție interioară: {innerExceptionMessage}\n{ex.StackTrace}");
            }
        }


        private string FormatBon()
        {
            string bonText = "";
            foreach (var produsBon in ProduseBon)
            {
                bonText += $"{produsBon.Cantitate} x {produsBon.Produs.Nume} .......... {produsBon.Subtotal * produsBon.Cantitate} lei\n";
            }
            bonText += $"Total .............................. {TotalBon} lei";
            return bonText;
        }

        private void CautareProduse()
        {
            using (var context = new SupermarketDbContext())
            {
                var rezultate = context.Produse.Include(p => p.Categorie)
                                               .Include(p => p.Producator)
                                               .Include(p => p.Stocuri)
                                               .Where(p => p.Nume.Contains(CriteriuCautare) ||
                                                           p.CodBare.Contains(CriteriuCautare) ||
                                                           p.Producator.Nume.Contains(CriteriuCautare) ||
                                                           p.Categorie.Nume.Contains(CriteriuCautare))
                                               .ToList();
                Produse = new ObservableCollection<Produs>(rezultate);
            }
        }
    }
}
