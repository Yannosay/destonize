using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;

namespace Destonize.Services
{
    public class TrayIconService : IDisposable
    {
        private NotifyIcon? _notifyIcon;
        private readonly Window _window;
        private readonly SettingsService _settingsService;

        public TrayIconService(Window window, SettingsService settingsService)
        {
            _window = window;
            _settingsService = settingsService;
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = SystemIcons.Application;
            _notifyIcon.Text = "Destonize";
            _notifyIcon.Visible = false;
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            _window.StateChanged += Window_StateChanged;
        }

        public void ShowTrayIcon()
        {
            if (_notifyIcon != null)
                _notifyIcon.Visible = true;
        }

        public void HideTrayIcon()
        {
            if (_notifyIcon != null)
                _notifyIcon.Visible = false;
        }

        private void Window_StateChanged(object? sender, EventArgs e)
        {
            if (!_settingsService.GetSettings().MinimizeToTray)
                return;

            if (_window.WindowState == WindowState.Minimized)
            {
                _window.Hide();
                ShowTrayIcon();
            }
        }

        private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
        {
            _window.Show();
            _window.WindowState = WindowState.Normal;
            HideTrayIcon();
        }

        public void Dispose()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }
        }
    }
}