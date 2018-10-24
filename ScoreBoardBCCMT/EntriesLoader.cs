using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace ScoreBoardBCCMT
{
    class EntriesLoader
    {
        /// <summary>
        /// Load entries from file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public List<Entry> GetEntries(string path)
        {
            List<Entry> entries = new List<Entry>();
            entries.Add(new Entry("dummy", null));
            return entries;
        }
    }
}
