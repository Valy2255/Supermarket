﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tema3Supermarket.ViewModels;

namespace Tema3Supermarket.Views
{
    /// <summary>
    /// Interaction logic for CasierView.xaml
    /// </summary>
    public partial class CasierView : Window
    {
        private int casierId;
        public CasierView()
        {
            InitializeComponent();
            DataContext = new CasierViewModel(casierId);
        }
    }
}
