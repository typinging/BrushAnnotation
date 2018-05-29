using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BrushAnnotation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BrushAnnotationWindow : Window
    {
        private static readonly string inkCanvasStyleKey = "InkCanvasStyle";
        public BrushAnnotationWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= MainWindow_Loaded;
            BAViewModel.IniContainer(this.BrushCoverage, this.FindResource(inkCanvasStyleKey));
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                string dPath = ((Array)e.Data.GetData(System.Windows.DataFormats.FileDrop)).GetValue(0).ToString();
                BAViewModel.SearchPictures(dPath);
            }
        }
    }
}
