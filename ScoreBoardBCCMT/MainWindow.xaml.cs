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
        List<Entry> list = null;
        ListBox entrylistBox = null;

        public MainWindow()
        {
            InitializeComponent();
            this.AddDynamicElements();
        }
        /// <summary>
        /// Add Elements that need to be update in the code
        /// </summary>
        private void AddDynamicElements()
        {
            string path = PromptDialog.Prompt("Entries File", "Prompt", inputType: PromptDialog.InputType.Text);
            list = EntriesLoader.GetEntries(path);
            this.entrylistBox = new ListBox();
            Thickness listBoxthickness_1 = new Thickness
            {
                Left = 50,
                Top = 170,
                Right = 1465,
                Bottom = 10
            };
            this.entrylistBox.Margin = listBoxthickness_1;
            Grid grid = (Grid)this.Content;
            grid.Children.Add(this.entrylistBox);
            foreach (Entry entry in list)
            {
                //Show it
                ListBoxItem item = new ListBoxItem
                {
                    Content = entry.Groupname,
                    FontSize = 25
                };
                this.entrylistBox.Items.Add(item);
            }
        }

        private void Button_PreviewMouseLeftButtonUp_Exit(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Button_PreviewMouseLeftButtonUp_Minimize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void TextBox_TextChanged_Search(object sender, TextChangedEventArgs e)
        {
            this.entrylistBox.Items.Clear();
            TextBox textBox = (TextBox)sender;
            foreach (Entry entry in list)
            {
                if(entry.Groupname.Contains(textBox.Text))
                {
                    //Show it
                    ListBoxItem item = new ListBoxItem
                    {
                        Content = entry.Groupname,
                        FontSize = 25
                    };
                    this.entrylistBox.Items.Add(item);
                }
            }
        }
    }
}
