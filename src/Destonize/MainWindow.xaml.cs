using System;
using System.Windows;
using System.Windows.Controls;
using Destonize.Services;
using Destonize.ViewModels;

namespace Destonize
{
    public partial class MainWindow : Window
    {
        private readonly SettingsService _settingsService;
        private readonly MainViewModel _viewModel;
        private readonly UndoService _undoService;
        private TrayIconService? _trayIconService;

        public MainWindow(SettingsService settingsService)
        {
            InitializeComponent();
            _settingsService = settingsService;
            _undoService = new UndoService();
            _viewModel = new MainViewModel(settingsService);
            DataContext = _viewModel;
            _viewModel.LoadDesktopFiles();
            Loaded += (s, e) => DarkTitleBarHelper.ApplyDarkTitleBar(this);

            VersionText.Text = "Destonize v" + VersionHelper.GetVersion();

            if (_settingsService.GetSettings().MinimizeToTray)
            {
                _trayIconService = new TrayIconService(this, _settingsService);
                _trayIconService.ShowTrayIcon();
            }
        }

        private void Organize_Click(object sender, RoutedEventArgs e)
        {
            var organizeWindow = new OrganizeWindow(_settingsService);
            organizeWindow.Owner = this;
            organizeWindow.ShowDialog();
            _viewModel.LoadDesktopFiles();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(_settingsService);
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
            _viewModel.LoadDesktopFiles();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadDesktopFiles();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (_undoService.UndoLastMove())
            {
                DarkMessageBox.Show("Last move undone.", "Destonize");
                _viewModel.LoadDesktopFiles();
            }
            else
            {
                DarkMessageBox.Show("No moves to undo.", "Destonize");
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            _trayIconService?.Dispose();
            Close();
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            DarkMessageBox.Show("Destonize v" + VersionHelper.GetVersion() + "\nYannosay Productions", "About Destonize");
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.Filter(SearchBox.Text);
        }
    }
}