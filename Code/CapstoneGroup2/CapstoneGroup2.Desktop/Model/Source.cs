﻿using System;

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

        public string Type { get; set; }

        public SourceType NoteType => (SourceType)Enum.Parse(typeof(NoteType), this.Type, true);

        public string Name { get; set; }

        public bool IsLink { get; set; }

        public string Link { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}