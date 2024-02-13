using System;

namespace CapstoneGroup2.Desktop.Model
{
    public enum SourceType
    {
        #region Enum members

        Pdf = 1,
        Vid = 2

        #endregion
    }

    public class Source
    {
        #region Properties

        public int SourceId { get; set; }

        public string Username { get; set; }

        public string Type { get; set; }

        public SourceType NoteType => (SourceType)Enum.Parse(typeof(SourceType), this.Type, true);

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsLink { get; set; }

        public string Link { get; set; }

        public byte[] Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        #endregion
    }
}