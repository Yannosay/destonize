using System;
using System.Threading.Tasks;
using System.Windows;
using Destonize.Services;

namespace Destonize
{
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var settingsService = new SettingsService();
            var settings = settingsService.GetSettings();

            SplashWindow? splash = null;
            if (settings.ShowSplash)
            {
                splash = new SplashWindow();
                splash.Show();
                await Task.Delay(4000);
            }

            var mainWindow = new MainWindow(settingsService);
            mainWindow.Show();

            if (splash != null)
            {
                splash.Close();
            }

            if (settingsService.IsFirstRun())
            {
                var welcomeWindow = new WelcomeWindow(settingsService);
                welcomeWindow.Owner = mainWindow;
                var result = welcomeWindow.ShowDialog();
                if (result != true)
                {
                    mainWindow.Close();
                    Shutdown();
                    return;
                }
                settingsService.SetFirstRunComplete();
            }

            if (settings.AutoUpdate)
            {
                _ = Task.Run(async () =>
                {
                    var currentVersion = VersionHelper.GetVersion();
                    var latestVersion = await UpdateService.GetLatestVersionAsync(settings.UpdateSource);
                    if (latestVersion != null &&
                        Version.TryParse(latestVersion, out var latest) &&
                        Version.TryParse(currentVersion, out var current) &&
                        latest > current &&
                        settings.SkippedUpdateVersion != latestVersion &&
                        (settings.UpdateRemindAt == null || settings.UpdateRemindAt <= DateTime.Now))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            var updateWindow = new UpdateAvailableWindow(latestVersion, settings.UpdateSource, settingsService);
                            updateWindow.Owner = mainWindow;
                            updateWindow.ShowDialog();
                        });
                    }
                });
            }
        }
    }
}