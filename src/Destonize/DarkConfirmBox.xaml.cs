using System.Windows;
using Destonize.Services;

namespace Destonize
{
    public partial class DarkConfirmBox : Window
    {
        public DarkConfirmBox(string message, string title = "Confirm")
        {
            InitializeComponent();
            Title = title;
            MessageText.Text = message;
            Loaded += (s, e) => DarkTitleBarHelper.ApplyDarkTitleBar(this);
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}