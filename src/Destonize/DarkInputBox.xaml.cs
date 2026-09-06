using System.Windows;
using Destonize.Services;

namespace Destonize
{
    public partial class DarkInputBox : Window
    {
        public string InputText { get; private set; } = string.Empty;

        public DarkInputBox(string prompt, string title = "Input")
        {
            InitializeComponent();
            Title = title;
            PromptText.Text = prompt;
            Loaded += (s, e) => DarkTitleBarHelper.ApplyDarkTitleBar(this);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            InputText = InputBox.Text;
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