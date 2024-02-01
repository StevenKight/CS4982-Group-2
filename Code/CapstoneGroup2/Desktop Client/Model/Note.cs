using System.Collections.Generic;
using System.Linq;

namespace Desktop_Client.Model
{
    public class Note
    {
        public int SourceId { get; set; }

        public string Username { get; set; }

        public string NoteText { get; set; }

        public string TagsString { get; set; }

        public List<string> Tags => TagsString.Split(",").ToList();

        //public Source Source { get; set; }
    }
}