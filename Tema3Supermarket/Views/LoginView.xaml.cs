using System.Windows;
using Tema3Supermarket.ViewModels;

namespace Tema3Supermarket.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
    }
}
