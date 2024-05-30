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
    public class StocViewModel : BaseViewModel
    {
        private ObservableCollection<Stoc> stocuri;
        public ObservableCollection<Stoc> Stocuri
        {
            get { return stocuri; }
            set { stocuri = value; OnPropertyChanged("Stocuri"); }
        }

        private Stoc stocSelectat;
        public Stoc StocSelectat
        {
            get { return stocSelectat; }
            set { stocSelectat = value; OnPropertyChanged("StocSelectat"); }
        }

        public ICommand AddStocCommand { get; set; }
        public ICommand UpdateStocCommand { get; set; }
        public ICommand DeleteStocCommand { get; set; }

        public StocViewModel()
        {
            Stocuri = new ObservableCollection<Stoc>();
            LoadStocuri();

            AddStocCommand = new RelayCommand(param => AddStoc());
            UpdateStocCommand = new RelayCommand(param => UpdateStoc());
            DeleteStocCommand = new RelayCommand(param => DeleteStoc());
        }

        private void LoadStocuri()
        {
            using (var context = new SupermarketDbContext())
            {
                Stocuri = new ObservableCollection<Stoc>(context.Stocuri.Include(s => s.Produs).ToList());
            }
        }

        private void AddStoc()
        {
            using (var context = new SupermarketDbContext())
            {
                StocSelectat.PretVanzare = StocSelectat.PretAchizitie + (StocSelectat.PretAchizitie * GetAdaosComercial());
                context.Stocuri.Add(StocSelectat);
                context.SaveChanges();
                Stocuri.Add(StocSelectat);
            }
        }

        private void UpdateStoc()
        {
            using (var context = new SupermarketDbContext())
            {
                context.Entry(StocSelectat).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private void DeleteStoc()
        {
            using (var context = new SupermarketDbContext())
            {
                var stoc = context.Stocuri.Find(StocSelectat.Id);
                if (stoc != null)
                {
                    context.Stocuri.Remove(stoc);
                    context.SaveChanges();
                    Stocuri.Remove(StocSelectat);
                }
            }
        }

        private decimal GetAdaosComercial()
        {
            
            return 0.2m;
        }
    }
}
