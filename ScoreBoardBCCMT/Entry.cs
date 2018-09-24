using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreBoardBCCMT
{
    class Entry
    {
        public string Groupname { get; set; }
        public List<string> Participant { get; set; }

        public Entry(string groupname, List<string> participant)
        {
            Groupname = groupname;
            Participant = participant;
        }
    }
}
