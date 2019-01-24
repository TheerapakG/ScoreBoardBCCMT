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
using System.Timers;

namespace ScoreBoardBCCMT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int time = 4500; //FirstRound = 5400 Second = 4500
        private static System.Timers.Timer aTimer = null;
        List<Entry> list = null;
        ObservableCollection<string> searched = new ObservableCollection<string>();
        Entry select = null;
        Dictionary<string, Entry> nameEntry = null;
        Project project = null;

        int currHeight = 410;
        int currLeft = 0;

        public delegate void UpdateTextCallback(string message);
        public delegate void UpdateTextCallbackToo(string message);

        private void UpdateText(string message)
        {
            this.project.ShownTime.ShownTime.Content = message;
        }

        private void UpdateTextToo(string message)
        {
            this.Time.Content = message;
        }

        private void updateTimer()
        {
            TimeSpan t = TimeSpan.FromSeconds(time);
            this.Time.Dispatcher.Invoke(
                new UpdateTextCallbackToo(this.UpdateTextToo),
                new object[] { t.ToString() }
            );
            this.project.Dispatcher.Invoke(
                new UpdateTextCallback(this.UpdateText),
                new object[] { t.ToString() }
            );
        }

        public MainWindow()
        {
            InitializeComponent();
            this.AddDynamicElements();
            this.project = new Project();
            this.updateTimer();
            project.Show();
        }
        /// <summary>
        /// Add Elements that need to be update in the code
        /// </summary>
        private void AddDynamicElements()
        {
            this.list = new List<Entry>();
            this.nameEntry = new Dictionary<string, Entry>();
            this.entrylistBox.ItemsSource = searched;
        }

        private void UpdateTeam(Entry team)
        {
            team.Margin = new Thickness(currLeft, currHeight, 1912-310-(currLeft), 644-300-(currHeight-410));
            this.project.thegrid.Children.Add(team);
            currLeft += 315;
            if(1912 - 310 - (currLeft)<0)
            {
                currLeft = 0;
                currHeight += 305;
            }
        }

        public delegate void UpdateTeamCallback(Entry team);

        private void updateTeam(Entry team)
        {
            this.project.Dispatcher.Invoke(
                new UpdateTeamCallback(this.UpdateTeam),
                new object[] { team }
            );
        }

        private void Button_PreviewMoseLeftButtonUp_AddTeam(object sender, MouseButtonEventArgs e)
        {
            Entry entry = new Entry(this.Team.Text);
            if (this.list.Find(a => a.EntryName.Content.ToString() == entry.EntryName.Content.ToString()) != null)
            {
                this.Team.Text = "Already Added";
                return;
            }
            list.Add(entry);
            this.nameEntry.Add(entry.EntryName.Content.ToString(), entry);
            //Show it
            this.searched.Add(entry.EntryName.Content.ToString());
            this.updateTeam(entry);
            this.updateScore(entry);
        }

        private void Button_PreviewMouseLeftButtonUp_Exit(object sender, MouseButtonEventArgs e)
        {
            project.Close();
            this.Close();
        }

        private void Button_PreviewMouseLeftButtonUp_Minimize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void UpdateScoreDL(Entry team, int prob, int wrongamt, int correctime, bool update)
        {
            team.correctWrongs[prob - 1].wrongamt = wrongamt;
            team.correctWrongs[prob - 1].correctime = correctime;
            if (update)
            {
                team.ShownScore.Content = team.score.ToString("0.0");
                Trace.WriteLine("Update Screen!");
            }
        }

        public delegate void UpdateCallback(Entry team, int prob, int wrongamt, int correctime, bool update);

        private void updateScore(Entry team, int prob, int wrongamt, int correctime, bool update)
        {
            this.project.Dispatcher.Invoke(
                new UpdateCallback(this.UpdateScoreDL),
                new object[] { team, prob, wrongamt, correctime, update }
            );
        }

        private void Button_PreviewMouseLeftButtonUp_Upd(object sender, MouseButtonEventArgs e)
        {
            (sender as Button).Content = "Update";
            int prob = Convert.ToInt32((string) (Team_Pro.SelectedItem as ListBoxItem).Content);
            if(this.select == null)
            {
                (sender as Button).Content = "Choose a Team";
                return;
            }
            if (Team_Inc.Text == "")
            {
                (sender as Button).Content = "Check Your Input";
                return;
            }
            int wrong = Convert.ToInt32(Team_Inc.Text);
            int t = 0;
            if(Team_Hrs.Text == "" || Team_Min.Text == "" || Team_Sec.Text == "")
            {
                t = -1;
            }
            else
            {
                t = 3600 * Convert.ToInt32(Team_Hrs.Text) + 60 * Convert.ToInt32(Team_Min.Text) + Convert.ToInt32(Team_Sec.Text);
            }
            if (this.time > 900 || this.time == 0)
            {
                this.updateScore(this.select, prob, wrong, t, true);
            }
            else
            {
                this.updateScore(this.select, prob, wrong, t, false);
            }
            this.SelectionScoreView.Text = this.select.score.ToString();
        }

        private void TextBox_TextChanged_Search(object sender, TextChangedEventArgs e)
        {
            this.searched.Clear();
            TextBox textBox = (TextBox)sender;
            foreach (Entry entry in list)
            {
                if (textBox.Text.Split().All(entry.EntryName.Content.ToString().Contains))
                {
                    //Show it
                    this.searched.Add(entry.EntryName.Content.ToString());
                }
            }
            this.entrylistBox.ItemsSource = searched;
        }

        private void EntrylistBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string listBoxItem = ((sender as ListBox).SelectedItem as string);
            if (listBoxItem != null)
            {
                this.select = nameEntry[listBoxItem];
                this.SelectionNameView.Text = listBoxItem;
                this.SelectionScoreView.Text = this.select.score.ToString();
                this.Team_Inc.Text = this.select.correctWrongs[Convert.ToInt32((string)(Team_Pro.SelectedItem as ListBoxItem).Content)-1].wrongamt.ToString();
                if (this.select.correctWrongs[Convert.ToInt32((string)(Team_Pro.SelectedItem as ListBoxItem).Content)-1].correctime > 0)
                {
                    this.Team_Hrs.Text = (this.select.correctWrongs[Convert.ToInt32((string)(Team_Pro.SelectedItem as ListBoxItem).Content)-1].correctime / 3600).ToString();
                    this.Team_Min.Text = ((this.select.correctWrongs[Convert.ToInt32((string)(Team_Pro.SelectedItem as ListBoxItem).Content)-1].correctime / 60) % 60).ToString();
                    this.Team_Sec.Text = (this.select.correctWrongs[Convert.ToInt32((string)(Team_Pro.SelectedItem as ListBoxItem).Content)-1].correctime % 60).ToString();
                }
                else
                {
                    this.Team_Hrs.Text = "";
                    this.Team_Min.Text = "";
                    this.Team_Sec.Text = "";
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((sender as ComboBox).SelectedItem as ComboBoxItem).Content == null) return;
            this.Team_Inc.Text = this.select.correctWrongs[Convert.ToInt32((string)(Team_Pro.SelectedItem as ListBoxItem).Content) - 1].wrongamt.ToString();
            if (this.select.correctWrongs[Convert.ToInt32((string)(Team_Pro.SelectedItem as ListBoxItem).Content)-1].correctime > 0)
            {
                this.Team_Hrs.Text = (this.select.correctWrongs[Convert.ToInt32((string)(Team_Pro.SelectedItem as ListBoxItem).Content)-1].correctime / 3600).ToString();
                this.Team_Min.Text = ((this.select.correctWrongs[Convert.ToInt32((string)(Team_Pro.SelectedItem as ListBoxItem).Content)-1].correctime / 60) % 60).ToString();
                this.Team_Sec.Text = (this.select.correctWrongs[Convert.ToInt32((string)(Team_Pro.SelectedItem as ListBoxItem).Content)-1].correctime % 60).ToString();
            }
            else
            {
                this.Team_Hrs.Text = "";
                this.Team_Min.Text = "";
                this.Team_Sec.Text = "";
            }
        }

        private void Button_PreviewMoseLeftButtonUp_ST(object sender, RoutedEventArgs e)
        {
            (sender as Button).Content = "Already Started!";
            if(aTimer == null)
            {
                aTimer = new System.Timers.Timer();
                aTimer.Interval = 1000;
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Start();
            }
            Trace.WriteLine("Timer!");

        }

        private void UpdateDL(Entry team)
        {
            team.ShownScore.Content = team.score.ToString("0.0");
            Trace.WriteLine("Update Screen!");
        }

        public delegate void UCallback(Entry team);

        private void updateScore(Entry team)
        {
            this.project.Dispatcher.Invoke(
                new UCallback(this.UpdateDL),
                new object[] { team }
            );
        }

        private void UpdateFreeze(bool v)
        {
            if (v) this.project.Freeze.Visibility = Visibility.Visible;
            else this.project.Freeze.Visibility = Visibility.Hidden;
        }

        public delegate void UpdateFreezeCallback(bool v);

        private void updateFreeze(bool v)
        {
            this.project.Dispatcher.Invoke(
                new UpdateFreezeCallback(this.UpdateFreeze),
                new object[] { v }
            );
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (this.time > 0)
            {
                this.time -= 1;
                Trace.WriteLine(this.time);
                this.updateTimer();
            }
            else
            {
                if (!big)
                {
                    aTimer.Stop();
                    updateFreeze(false);
                    foreach (Entry el in list)
                    {
                        updateScore(el);
                    }
                }
            }

            if (this.time == 900 && !big)
            {
                updateFreeze(true);
            }
        }

        private void UpdateMode(bool big)
        {
            if (big)
            {
                this.project.ShownTime.ShownTime.FontSize = 250;
                this.project.ShownTime.Margin = new Thickness(0, 105, 0, 100);
            }
            else
            {
                this.project.ShownTime.ShownTime.FontSize = 150;
                this.project.ShownTime.Margin = new Thickness(0, 205, 0, 644);
            }
        }

        public delegate void UpdateModeCallback(bool big);

        private void updateMode(bool big)
        {
            this.project.Dispatcher.Invoke(
                new UpdateModeCallback(UpdateMode),
                new object[] { big }
            );
        }

        bool big = false;

        private void Button_PreviewMoseLeftButtonUp_SM(object sender, RoutedEventArgs e)
        {
            updateMode(!big);
            big = !big;
        }
    }
}
