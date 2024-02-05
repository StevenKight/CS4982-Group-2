using System;

namespace CapstoneGroup2.Desktop.Model
{
    public enum NoteType
    {
        #region Enum members

        Pdf = 1,
        Vid = 2

        #endregion
    }

    public class UserNote
    {
        #region Properties

        public int Id { get; set; }

        public string Note { get; set; }

        public string Type { get; set; }

        public NoteType NoteType => (NoteType)Enum.Parse(typeof(NoteType), this.Type, true);

        #endregion
    }
}