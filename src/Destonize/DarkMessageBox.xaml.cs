using System.Windows;
using Destonize.Services;

namespace Destonize
{
    public partial class DarkMessageBox : Window
    {
        public DarkMessageBox(string message, string title = "Destonize")
        {
            InitializeComponent();
            Title = title;
            MessageText.Text = message;
            Loaded += (s, e) => DarkTitleBarHelper.ApplyDarkTitleBar(this);
        }

        public static void Show(string message, string title = "Destonize")
        {
            var box = new DarkMessageBox(message, title);
            box.ShowDialog();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}