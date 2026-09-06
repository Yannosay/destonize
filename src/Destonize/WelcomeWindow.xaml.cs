using System.Windows;
using Destonize.Services;

namespace Destonize
{
    public partial class WelcomeWindow : Window
    {
        private readonly SettingsService _settingsService;

        public WelcomeWindow(SettingsService settingsService)
        {
            InitializeComponent();
            _settingsService = settingsService;
            AcceptCheckBox.Checked += (s, e) => ContinueButton.IsEnabled = true;
            AcceptCheckBox.Unchecked += (s, e) => ContinueButton.IsEnabled = false;
            Loaded += (s, e) => DarkTitleBarHelper.ApplyDarkTitleBar(this);
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}