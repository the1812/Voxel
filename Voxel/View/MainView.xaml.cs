using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Voxel.Model;
using Voxel.ViewModel;
using static Voxel.Model.Settings;

namespace Voxel.View
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            var showSplashScreen = GetBoolean(ShowSplashScreenKey);
            if (showSplashScreen)
            {
                this.showSplashScreen();
            }
            else
            {
                loadAllView();
            }
            InitializeComponent();
            DataContext = new MainViewModel(this);
            Loaded += (s, e) => { dpi = this.GetDpi(); };
        }

        public NonscalableTileView NonscalableTileView { get; private set; }
        public ImageTileView ImageTileView { get; private set; }
        private void loadAllView()
        {
            NonscalableTileView = new NonscalableTileView();
            ImageTileView = new ImageTileView();
        }


        private void showSplashScreen()
        {
            var splash = getSplashScreen();
            splash.Show(false);
            loadAllView();
            //Thread.Sleep(500);
            splash.Close(TimeSpan.FromMilliseconds(200));
        }
        private SplashScreen getSplashScreen()
        {
            var dpi = VisualTreeHelper.GetDpi(this);
            var resourcePath = "Voxel Background";
            if (dpi.DpiScaleX <= 1.00)
            {
                resourcePath += "@0,25x";
            }
            else if (dpi.DpiScaleX <= 1.50)
            {
                resourcePath += "@0,375x";
            }
            else if (dpi.DpiScaleX <= 2.00)
            {
                resourcePath += "@0,5x";
            }
            resourcePath += ".png";
            return new SplashScreen(resourcePath);
        }

        private static Point dpi;
        public static Point Dpi => dpi;
    }
}
