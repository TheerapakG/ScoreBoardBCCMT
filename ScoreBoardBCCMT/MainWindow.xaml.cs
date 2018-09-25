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
using System.Collections.ObjectModel;

namespace ScoreBoardBCCMT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Entry> list = null;
        ObservableCollection<string> searched = new ObservableCollection<string>();
        Entry select = null;
        Dictionary<string, Entry> nameEntry = null;

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
            this.list = EntriesLoader.GetEntries(path);
            if(this.list.Count() == 0)
            {
                throw new Exception("entries list count equal zero");
            }
            this.nameEntry = new Dictionary<string, Entry>();
            this.entrylistBox.ItemsSource = searched;
            foreach (Entry entry in list)
            {
                this.nameEntry.Add(entry.Groupname, entry);
                //Show it
                this.searched.Add(entry.Groupname);
            }
            this.select = list[0];
            this.SelectionNameView.Text = list[0].Groupname;
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
            this.searched.Clear();
            TextBox textBox = (TextBox)sender;
            foreach (Entry entry in list)
            {
                if(textBox.Text.Split().All(entry.Groupname.Contains))
                {
                    //Show it
                    this.searched.Add(entry.Groupname);
                }
            }
            this.entrylistBox.ItemsSource = searched;
        }

        private void EntrylistBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string listBoxItem = ((sender as ListBox).SelectedItem as string);
            if(listBoxItem != null)
            {
                this.select = nameEntry[listBoxItem];
                this.SelectionNameView.Text = listBoxItem;
            }
        }
    }
}
