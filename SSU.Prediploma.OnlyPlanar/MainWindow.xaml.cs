using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SSU.Prediploma.GraphDrawer;

namespace SSU.Prediploma.OnlyPlanar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnCreateGraphClick(object sender, RoutedEventArgs e)
        {
            int fl;
            DrawMyGraph graphDrawer = new DrawMyGraph(textBox.Text.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray(), 2, out fl);
            image.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                graphDrawer.picture.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(graphDrawer.picture.Width, graphDrawer.picture.Height));
        }
    }
}
