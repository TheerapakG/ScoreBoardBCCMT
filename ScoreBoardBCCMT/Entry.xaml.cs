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

namespace ScoreBoardBCCMT
{
    /// <summary>
    /// Interaction logic for Entry.xaml
    /// </summary>
    public partial class Entry : UserControl
    {
        public class CorrectWrong
        {
            public int correctime = -1;
            public int wrongamt = 0;
        }

        public List<CorrectWrong> correctWrongs = new List<CorrectWrong>();

        public double score
        {
            get
            {
                double sum = 0;
                foreach (CorrectWrong el in correctWrongs)
                {
                    sum += -8 * el.wrongamt;
                    if (el.correctime >= 0)
                    {
                        sum += 5 * Math.PI + (((double)(el.correctime)) / 4500) * 5 * Math.PI;
                    }
                }
                return sum;
            }
        }
        public Entry(string name)
        {
            InitializeComponent();
            this.EntryName.Content = name;
            for (int i = 0; i < 16; i++)
            {
                this.correctWrongs.Add(new CorrectWrong());
            }
        }
    }
}
