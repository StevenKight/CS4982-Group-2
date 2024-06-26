﻿using System;
using System.Collections.Generic;

namespace CapstoneGroup2.Desktop.Library.Model
{
    /// <summary>
    ///     The note model class
    /// </summary>
    public class Note
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the note identifier.
        /// </summary>
        /// <value>
        ///     The note identifier.
        /// </value>
        public int NoteId { get; set; }

        /// <summary>
        ///     Gets or sets the source identifier.
        /// </summary>
        /// <value>
        ///     The source identifier.
        /// </value>
        public int SourceId { get; set; }

        /// <summary>
        ///     Gets or sets the username.
        /// </summary>
        /// <value>
        ///     The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        ///     Gets or sets the note text.
        /// </summary>
        /// <value>
        ///     The note text.
        /// </value>
        public string NoteText { get; set; }

        /// <summary>
        ///     Gets the tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public List<Tag>? Tags { get; set; }

        /// <summary>
        ///     Gets or sets the note date.
        /// </summary>
        /// <value>
        ///     The note date.
        /// </value>
        public DateTime NoteDate { get; set; }

        #endregion
    }
}