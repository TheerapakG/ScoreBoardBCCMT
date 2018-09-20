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
using System.Diagnostics;

namespace ScoreBoardBCCMT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Entry> list = new List<Entry>();

        public MainWindow()
        {
            Trace.WriteLine("new MainWindow");
            this.AddDynamicElements();
            InitializeComponent();
        }

        private void AddDynamicElements()
        {
            ListBox listBox = new ListBox();
            Thickness listBoxthickness = new Thickness();
            listBoxthickness.Left = 50;
            listBoxthickness.Top = 170;
            listBoxthickness.Right = 1465;
            listBoxthickness.Bottom = 10;
            listBox.Margin = listBoxthickness;
        }

        private void Button_PreviewMouseLeftButtonUp_Exit(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Button_PreviewMouseLeftButtonUp_Minimize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
