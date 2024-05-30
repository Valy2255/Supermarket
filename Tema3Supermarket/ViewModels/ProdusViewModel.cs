using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tema3Supermarket.Comands;
using Tema3Supermarket.Models;

namespace Tema3Supermarket.ViewModels
{
    public class ProdusViewModel : BaseViewModel
    {
        private ObservableCollection<Produs> produse;
        public ObservableCollection<Produs> Produse
        {
            get { return produse; }
            set { produse = value; OnPropertyChanged("Produse"); }
        }

        private Produs produsSelectat;
        public Produs ProdusSelectat
        {
            get { return produsSelectat; }
            set { produsSelectat = value; OnPropertyChanged("ProdusSelectat"); }
        }

        public ICommand AddProdusCommand { get; set; }
        public ICommand UpdateProdusCommand { get; set; }
        public ICommand DeleteProdusCommand { get; set; }

        public ProdusViewModel()
        {
            Produse = new ObservableCollection<Produs>();
            LoadProduse();

            AddProdusCommand = new RelayCommand(param => AddProdus());
            UpdateProdusCommand = new RelayCommand(param => UpdateProdus());
            DeleteProdusCommand = new RelayCommand(param => DeleteProdus());
        }

        private void LoadProduse()
        {
            using (var context = new SupermarketDbContext())
            {
                Produse = new ObservableCollection<Produs>(context.Produse.Include(p => p.Categorie).Include(p => p.Producator).ToList());
            }
        }

        private void AddProdus()
        {
            using (var context = new SupermarketDbContext())
            {
                context.Produse.Add(ProdusSelectat);
                context.SaveChanges();
                Produse.Add(ProdusSelectat);
            }
        }

        private void UpdateProdus()
        {
            using (var context = new SupermarketDbContext())
            {
                context.Entry(ProdusSelectat).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private void DeleteProdus()
        {
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
    }


}
