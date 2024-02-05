using System.Collections.Generic;
using System.Linq;

namespace CapstoneGroup2.Desktop.Model
{
    public class Note
    {
        #region Properties

        public int SourceId { get; set; }

        public string Username { get; set; }

        public string NoteText { get; set; }

        public string TagsString { get; set; }

        public List<string> Tags => this.TagsString.Split(",").ToList();

        public Source Source { get; set; }

        #endregion
    }
}