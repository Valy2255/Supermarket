using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Tema3Supermarket.Comands;
using Tema3Supermarket.Models;
using System.Data.Entity;

namespace Tema3Supermarket.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private const decimal AdaosComercial = 0.2m; // 20% adaos comercial
        public DateTime CurrentDate => DateTime.Now;

        private ObservableCollection<Produs> produse;
        public ObservableCollection<Produs> Produse
        {
            get { return produse; }
            set { produse = value; OnPropertyChanged("Produse"); }
        }

        private ObservableCollection<Categorie> categorii;
        public ObservableCollection<Categorie> Categorii
        {
            get { return categorii; }
            set { categorii = value; OnPropertyChanged("Categorii"); }
        }

        private ObservableCollection<Producator> producatori;
        public ObservableCollection<Producator> Producatori
        {
            get { return producatori; }
            set { producatori = value; OnPropertyChanged("Producatori"); }
        }

        private ObservableCollection<Stoc> stocuri;
        public ObservableCollection<Stoc> Stocuri
        {
            get { return stocuri; }
            set { stocuri = value; OnPropertyChanged("Stocuri"); }
        }

        private Categorie categorieSelectata;
        public Categorie CategorieSelectata
        {
            get { return categorieSelectata; }
            set { categorieSelectata = value; OnPropertyChanged("CategorieSelectata"); }
        }

        private Producator producatorSelectat;
        public Producator ProducatorSelectat
        {
            get { return producatorSelectat; }
            set { producatorSelectat = value; OnPropertyChanged("ProducatorSelectat"); }
        }

        private Produs produsSelectat;
        public Produs ProdusSelectat
        {
            get { return produsSelectat; }
            set { produsSelectat = value; OnPropertyChanged("ProdusSelectat"); }
        }

        private Stoc stocSelectat;
        public Stoc StocSelectat
        {
            get { return stocSelectat; }
            set { stocSelectat = value; OnPropertyChanged("StocSelectat"); }
        }

        private string numeProdus;
        public string NumeProdus
        {
            get { return numeProdus; }
            set { numeProdus = value; OnPropertyChanged("NumeProdus"); }
        }

        private string codBareProdus;
        public string CodBareProdus
        {
            get { return codBareProdus; }
            set { codBareProdus = value; OnPropertyChanged("CodBareProdus"); }
        }

        private int cantitateStoc;
        public int CantitateStoc
        {
            get { return cantitateStoc; }
            set { cantitateStoc = value; OnPropertyChanged("CantitateStoc"); }
        }

        private string unitateMasuraStoc;
        public string UnitateMasuraStoc
        {
            get { return unitateMasuraStoc; }
            set { unitateMasuraStoc = value; OnPropertyChanged("UnitateMasuraStoc"); }
        }

        private DateTime dataAprovizionareStoc = DateTime.Now;
        public DateTime DataAprovizionareStoc
        {
            get { return dataAprovizionareStoc; }
            set { dataAprovizionareStoc = value; OnPropertyChanged("DataAprovizionareStoc"); }
        }

        private DateTime dataExpirareStoc = DateTime.Now;
        public DateTime DataExpirareStoc
        {
            get { return dataExpirareStoc; }
            set { dataExpirareStoc = value; OnPropertyChanged("DataExpirareStoc"); }
        }

        private decimal pretAchizitieStoc;
        public decimal PretAchizitieStoc
        {
            get { return pretAchizitieStoc; }
            set
            {
                pretAchizitieStoc = value;
                OnPropertyChanged("PretAchizitieStoc");
                CalculeazaPretVanzare();
            }
        }

        private decimal pretVanzareStoc;
        public decimal PretVanzareStoc
        {
            get { return pretVanzareStoc; }
            set { pretVanzareStoc = value; OnPropertyChanged("PretVanzareStoc"); }
        }

        private ObservableCollection<dynamic> valoareCategorii;
        public ObservableCollection<dynamic> ValoareCategorii
        {
            get { return valoareCategorii; }
            set { valoareCategorii = value; OnPropertyChanged("ValoareCategorii"); }
        }

        private Utilizator utilizatorSelectat;
        public Utilizator UtilizatorSelectat
        {
            get { return utilizatorSelectat; }
            set { utilizatorSelectat = value; OnPropertyChanged("UtilizatorSelectat"); }
        }

        private DateTime? lunaSelectata;
        public DateTime? LunaSelectata
        {
            get { return lunaSelectata; }
            set { lunaSelectata = value; OnPropertyChanged("LunaSelectata"); }
        }

        private ObservableCollection<dynamic> vanzariZilnice;
        public ObservableCollection<dynamic> VanzariZilnice
        {
            get { return vanzariZilnice; }
            set { vanzariZilnice = value; OnPropertyChanged("VanzariZilnice"); }
        }
        private DateTime? dataSelectata;
        public DateTime? DataSelectata
        {
            get { return dataSelectata; }
            set { dataSelectata = value; OnPropertyChanged("DataSelectata"); }
        }

        private BonCasa bonSelectat;
        public BonCasa BonSelectat
        {
            get { return bonSelectat; }
            set { bonSelectat = value; OnPropertyChanged("BonSelectat"); }
        }

        private ObservableCollection<Utilizator> utilizatori;
        public ObservableCollection<Utilizator> Utilizatori
        {
            get { return utilizatori; }
            set
            {
                utilizatori = value;
                OnPropertyChanged("Utilizatori");
            }
        }







        public ICommand ListareProdusePeCategoriiCommand { get; set; }
        public ICommand CalculeazaValoareCategoriiCommand { get; set; }
        public ICommand AdaugaProdusCommand { get; set; }
        public ICommand ActualizeazaProdusCommand { get; set; }
        public ICommand StergeProdusCommand { get; set; }
        public ICommand AdaugaStocCommand { get; set; }
        public ICommand ActualizeazaStocCommand { get; set; }

        public ICommand VizualizareStocuriCommand { get; set; }

        public ICommand ListareProduseProducatorCommand { get; private set; }
        public ICommand VizualizareVanzariUtilizatorCommand { get; private set; }

        
        public ICommand AfișareBonMaximZiCommand { get; private set; }

        
        public AdminViewModel()
        {
            Produse = new ObservableCollection<Produs>();
            Categorii = new ObservableCollection<Categorie>();
            Producatori = new ObservableCollection<Producator>();
            Stocuri = new ObservableCollection<Stoc>();
            ValoareCategorii = new ObservableCollection<dynamic>();
            LoadCategorii();
            LoadProducatori();
            LoadProduse();
            LoadUtilizatori();

            ListareProduseProducatorCommand = new RelayCommand(param => ListareProduseProducator());
            ListareProdusePeCategoriiCommand = new RelayCommand(param => ListareProdusePeCategorii());
            CalculeazaValoareCategoriiCommand = new RelayCommand(param => CalculeazaValoareCategorii());
            AdaugaProdusCommand = new RelayCommand(param => AdaugaProdus());
            ActualizeazaProdusCommand = new RelayCommand(param => ActualizeazaProdus());
            StergeProdusCommand = new RelayCommand(param => StergeProdus());
            AdaugaStocCommand = new RelayCommand(param => AdaugaStoc());
            ActualizeazaStocCommand = new RelayCommand(param => ActualizeazaStoc());
            VizualizareStocuriCommand = new RelayCommand(param => VizualizareStocuri());
            
            VizualizareVanzariUtilizatorCommand = new RelayCommand(param => VizualizareVanzariUtilizator());
           
            AfișareBonMaximZiCommand = new RelayCommand(param => AfisareBonMaximZi());
        }

        private void LoadUtilizatori()
        {
            using (var context = new SupermarketDbContext())
            {
                Utilizatori = new ObservableCollection<Utilizator>(context.Utilizatori.ToList());
            }
        }


        private void VizualizareStocuri()
        {
            LoadStocuri();
        }
        private void LoadProduse()
        {
            using (var context = new SupermarketDbContext())
            {
                Produse = new ObservableCollection<Produs>(context.Produse.Include(p => p.Categorie).Include(p => p.Producator).ToList());
            }
        }

        private void LoadCategorii()
        {
            using (var context = new SupermarketDbContext())
            {
                Categorii = new ObservableCollection<Categorie>(context.Categorii.ToList());
            }
        }

        private void LoadProducatori()
        {
            using (var context = new SupermarketDbContext())
            {
                Producatori = new ObservableCollection<Producator>(context.Producatori.ToList());
            }
        }

        private void CalculeazaPretVanzare()
        {
            PretVanzareStoc = PretAchizitieStoc + (PretAchizitieStoc * AdaosComercial);
        }

        private void ListareProduseProducator()
        {
            if (ProducatorSelectat == null)
            {
                MessageBox.Show("Selectați un producător.");
                return;
            }

            using (var context = new SupermarketDbContext())
            {
                Produse = new ObservableCollection<Produs>(context.Produse
                    .Include(p => p.Categorie)
                    .Where(p => p.ProducatorId == ProducatorSelectat.Id)
                    .OrderBy(p => p.Categorie.Nume)
                    .ToList());
            }
        }
        public void ListareProdusePeCategorii()
        {
            if (CategorieSelectata == null)
            {
                MessageBox.Show("Selectați o categorie.");
                return;
            }

            using (var context = new SupermarketDbContext())
            {
                Produse = new ObservableCollection<Produs>(context.Produse.Include(p => p.Categorie).Include(p => p.Producator)
                    .Where(p => p.CategorieId == CategorieSelectata.Id).ToList());
            }
        }
        private void VizualizareVanzariUtilizator()
        {
            if (UtilizatorSelectat == null || LunaSelectata == null)
            {
                MessageBox.Show("Selectați un utilizator și o lună.");
                return;
            }

            using (var context = new SupermarketDbContext())
            {
                var vanzari = context.BonuriCasa
                    .Where(b => b.CasierId == UtilizatorSelectat.Id && b.DataEliberare.Month == LunaSelectata.Value.Month)
                    .GroupBy(b => b.DataEliberare.Day)
                    .Select(group => new
                    {
                        Ziua = group.Key,
                        Total = group.Sum(b => b.ProduseBon.Sum(pb => pb.Subtotal))
                    })
                    .ToList();

                VanzariZilnice = new ObservableCollection<dynamic>(vanzari);
            }
        }


        private void AfisareBonMaximZi()
        {
            if (DataSelectata == null)
            {
                MessageBox.Show("Selectați o dată.");
                return;
            }

            using (var context = new SupermarketDbContext())
            {
                var bonMaxim = context.BonuriCasa
                    .Where(b => DbFunctions.TruncateTime(b.DataEliberare) == DbFunctions.TruncateTime(DataSelectata.Value))
                    .Select(b => new
                    {
                        Bon = b,
                        TotalSubtotal = b.ProduseBon.Sum(pb => (decimal?)pb.Subtotal) 
                    })
                    .OrderByDescending(b => b.TotalSubtotal ?? 0) 
                    .FirstOrDefault();

                if (bonMaxim != null && bonMaxim.TotalSubtotal != null)
                {
                    BonSelectat = bonMaxim.Bon;
                    MessageBox.Show($"Cea mai mare sumă încasată în ziua selectată este: {bonMaxim.TotalSubtotal}.");
                }
                else
                {
                    MessageBox.Show("Nu există bonuri pentru ziua selectată sau nu sunt sume înregistrate.");
                }
            }
        }



        public void CalculeazaValoareCategorii()
        {
            using (var context = new SupermarketDbContext())
            {
                var categoriiValoare = context.Categorii.Include(c => c.Produse)
                    .Select(c => new
                    {
                        Categorie = c.Nume,
                        Valoare = c.Produse.Sum(p => (decimal?)p.Stocuri.Sum(s => s.PretVanzare * s.Cantitate)) ?? 0M
                    }).ToList();

                ValoareCategorii = new ObservableCollection<dynamic>(categoriiValoare);
            }

        }

        private void AdaugaProdus()
        {
            if (CategorieSelectata == null || ProducatorSelectat == null)
            {
                MessageBox.Show("Selectați o categorie și un producător.");
                return;
            }

            using (var context = new SupermarketDbContext())
            {
                
                var categorie = context.Categorii.FirstOrDefault(c => c.Nume == CategorieSelectata.Nume) ?? CategorieSelectata;
                var producator = context.Producatori.FirstOrDefault(p => p.Nume == ProducatorSelectat.Nume) ?? ProducatorSelectat;

                var produsNou = new Produs
                {
                    Nume = NumeProdus,
                    CodBare = CodBareProdus,
                    Categorie = categorie,
                    Producator = producator
                };

                context.Produse.Add(produsNou);
                context.SaveChanges();
                Produse.Add(produsNou);

                NumeProdus = string.Empty;
                CodBareProdus = string.Empty;
                CategorieSelectata = null;
                ProducatorSelectat = null;
            }
        }


        private void ActualizeazaProdus()
        {
            if (ProdusSelectat == null)
            {
                MessageBox.Show("Selectați un produs.");
                return;
            }

            using (var context = new SupermarketDbContext())
            {
                
                var produs = context.Produse.Find(ProdusSelectat.Id);
                if (produs == null)
                {
                    
                    context.Produse.Attach(ProdusSelectat); 
                    context.Entry(ProdusSelectat).State = EntityState.Modified; 
                }
                else
                {
                    
                    produs.Nume = ProdusSelectat.Nume;
                    produs.CodBare = ProdusSelectat.CodBare;
                    produs.CategorieId = ProdusSelectat.CategorieId;
                    produs.ProducatorId = ProdusSelectat.ProducatorId;
                }

                
                context.SaveChanges();

                
                LoadProduse();
            }
        }


        private void StergeProdus()
        {
            if (ProdusSelectat == null)
            {
                MessageBox.Show("Selectați un produs.");
                return;
            }

            using (var context = new SupermarketDbContext())
            {
                var produs = context.Produse.Find(ProdusSelectat.Id);
                if (produs != null)
                {
                    context.Produse.Remove(produs);
                    context.SaveChanges();
                    Produse.Remove(ProdusSelectat);
                }
            }
        }

        private void AdaugaStoc()
        {
            if (ProdusSelectat == null)
            {
                MessageBox.Show("Selectați un produs.");
                return;
            }

            using (var context = new SupermarketDbContext())
            {
                
                var stocExistent = context.Stocuri.FirstOrDefault(s => s.ProdusId == ProdusSelectat.Id && s.DataAprovizionare == DataAprovizionareStoc);
                if (stocExistent != null)
                {
                    stocExistent.Cantitate += CantitateStoc; 
                }
                else
                {
                    var stocNou = new Stoc
                    {
                        ProdusId = ProdusSelectat.Id,
                        Cantitate = CantitateStoc,
                        UnitateMasura = UnitateMasuraStoc,
                        DataAprovizionare = DataAprovizionareStoc,
                        DataExpirare = DataExpirareStoc,
                        PretAchizitie = PretAchizitieStoc,
                        PretVanzare = PretVanzareStoc
                    };
                    context.Stocuri.Add(stocNou);
                }
                context.SaveChanges();
                LoadStocuri();
                ResetStocFields();
            }
        }

        private void ResetStocFields()
        {
            CantitateStoc = 0;
            UnitateMasuraStoc = string.Empty;
            DataAprovizionareStoc = DateTime.Now;
            DataExpirareStoc = DateTime.Now;
            PretAchizitieStoc = 0;
            PretVanzareStoc = 0;
        }


        private void ActualizeazaStoc()
        {
            if (StocSelectat == null)
            {
                MessageBox.Show("Selectați un stoc.");
                return;
            }

            using (var context = new SupermarketDbContext())
            {
                var stoc = context.Stocuri.Find(StocSelectat.Id);
                if (stoc != null)
                {
                    stoc.Cantitate = CantitateStoc;
                    stoc.UnitateMasura = UnitateMasuraStoc;
                    stoc.DataAprovizionare = DataAprovizionareStoc;
                    stoc.DataExpirare = DataExpirareStoc;
                    stoc.PretAchizitie = PretAchizitieStoc;
                    stoc.PretVanzare = PretVanzareStoc;
                    context.SaveChanges();
                    LoadStocuri();
                }
            }
        }

        private void LoadStocuri()
        {
            using (var context = new SupermarketDbContext())
            {
                Stocuri = new ObservableCollection<Stoc>(context.Stocuri.Include(s => s.Produs).ToList());
            }
        }
    }
}
