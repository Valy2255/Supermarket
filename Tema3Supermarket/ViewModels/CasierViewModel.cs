using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Tema3Supermarket.Comands;
using Tema3Supermarket.Models;
using System.Data.Entity;
using System;
using System.Data.Entity.Validation;

namespace Tema3Supermarket.ViewModels
{
    public class CasierViewModel : BaseViewModel
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

        private int casierId;
        public int CasierId
        {
            get { return casierId; }
            set { casierId = value; OnPropertyChanged("CasierId"); }
        }

        public ICommand IncarcaBonuriCommand { get; private set; }

        public ICommand AdaugaProdusPeBonCommand { get; set; }
        public ICommand EmitereBonCommand { get; set; }
        public ICommand CautareProduseCommand { get; set; }

        public CasierViewModel(int casierId)
        {
            Produse = new ObservableCollection<Produs>();
            ProduseBon = new ObservableCollection<ProdusBon>();
            CasierId = casierId;
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
            if (stoc == null)
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
            ProduseBon.Add(produsBon);
            CalculeazaTotal();
        }


        private void CalculeazaTotal()
        {
            TotalBon = ProduseBon.Sum(pb => pb.Subtotal);
            MessageBox.Show($"Total Bon: {TotalBon}");
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
                            CasierId = this.CasierId, 
                            SumaIncasata = TotalBon
                        };

                        context.BonuriCasa.Add(bon);
                        context.SaveChanges();  

                        foreach (var produsBon in ProduseBon)
                        {
                            produsBon.BonId = bon.Id;
                            context.ProduseBon.Add(produsBon);

                            
                            var stoc = context.Stocuri.FirstOrDefault(s => s.ProdusId == produsBon.ProdusId);
                            if (stoc != null && stoc.Cantitate >= produsBon.Cantitate)
                            {
                                stoc.Cantitate -= produsBon.Cantitate;  
                                context.Entry(stoc).State = EntityState.Modified;
                            }
                            else
                            {
                                MessageBox.Show($"Stoc insuficient pentru produsul {produsBon.Produs.Nume}.", "Eroare Stoc", MessageBoxButton.OK, MessageBoxImage.Error);
                                continue;
                            }
                        }

                        context.SaveChanges();
                        transaction.Commit();  

                        MessageBox.Show("Bonul a fost emis cu succes.");
                        ProduseBon.Clear();
                        TotalBon = 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                MessageBox.Show($"Eroare la emiterea bonului: {fullErrorMessage}", "Eroare Validare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la emiterea bonului: {ex.Message}", "Eroare Sistem", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
