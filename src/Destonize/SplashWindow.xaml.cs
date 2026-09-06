using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Destonize.Services;

namespace Destonize
{
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            Loaded += SplashWindow_Loaded;
            Loaded += (s, e) => DarkTitleBarHelper.ApplyDarkTitleBar(this);
        }

        private void SplashWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "splash-screen.png");
            if (File.Exists(imagePath))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(imagePath, UriKind.Absolute);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                if (FindName("SplashImage") is Image imageControl)
                {
                    imageControl.Source = image;
                }
            }

            if (FindName("VersionText") is TextBlock versionText)
            {
                versionText.Text = "v" + VersionHelper.GetVersion();
            }
        }
    }
}