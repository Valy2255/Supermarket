using System.Windows;
using Tema3Supermarket.Views;

namespace Tema3Supermarket
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginView = new LoginView();
            loginView.Show();
        }
    }
}
