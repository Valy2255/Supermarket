using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tema3Supermarket.Controls
{
    public partial class PasswordTextBox : UserControl
    {
        public PasswordTextBox()
        {
            InitializeComponent();
            PasswordDisplay = string.Empty;
            Password = string.Empty;
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordTextBox), new PropertyMetadata(string.Empty));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        private string passwordDisplay;

        public string PasswordDisplay
        {
            get { return passwordDisplay; }
            set
            {
                passwordDisplay = value;
                passwordBox.Text = new string('*', passwordDisplay.Length);
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
            Password += e.Text;
            PasswordDisplay += e.Text;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && Password.Length > 0)
            {
                Password = Password.Substring(0, Password.Length - 1);
                PasswordDisplay = PasswordDisplay.Substring(0, PasswordDisplay.Length - 1);
                e.Handled = true;
            }
        }
    }
}
