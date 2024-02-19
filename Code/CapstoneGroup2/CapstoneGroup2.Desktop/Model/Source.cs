using System;
using System.Collections.Generic;
using System.Linq;

namespace CapstoneGroup2.Desktop.Model
{
    /// <summary>
    ///     Source type enums
    /// </summary>
    public enum SourceType
    {
        #region Enum members

        /// <summary>
        ///     The PDF
        /// </summary>
        Pdf = 1,

        /// <summary>
        ///     The vid
        /// </summary>
        Vid = 2

        #endregion
    }

    /// <summary>
    ///     The Source Model class
    /// </summary>
    public class Source
    {
        #region Properties

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
        ///     Gets or sets the type.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        ///     Gets the type of the note.
        /// </summary>
        /// <value>
        ///     The type of the note.
        /// </value>
        public SourceType NoteType => (SourceType)Enum.Parse(typeof(SourceType), this.Type, true);

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is link.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is link; otherwise, <c>false</c>.
        /// </value>
        public bool IsLink { get; set; }

        /// <summary>
        ///     Gets or sets the link.
        /// </summary>
        /// <value>
        ///     The link.
        /// </value>
        public string Link { get; set; }

        /// <summary>
        ///     Gets or sets the content.
        /// </summary>
        /// <value>
        ///     The content.
        /// </value>
        public byte[] Content { get; set; }

        /// <summary>
        ///     Gets or sets the created at.
        /// </summary>
        /// <value>
        ///     The created at.
        /// </value>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     Gets or sets the updated at.
        /// </summary>
        /// <value>
        ///     The updated at.
        /// </value>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        ///     Gets or sets the authors string.
        /// </summary>
        /// <value>
        ///     The authors string.
        /// </value>
        public string? AuthorsString { get; set; }

        /// <summary>
        ///     Gets the authors.
        /// </summary>
        /// <value>
        ///     The authors.
        /// </value>
        public List<string> Authors => this.AuthorsString.Split("|").ToList();

        /// <summary>
        ///     Gets or sets the publisher.
        /// </summary>
        /// <value>
        ///     The publisher.
        /// </value>
        public string? Publisher { get; set; }

        /// <summary>
        ///     Gets or sets the accessed at.
        /// </summary>
        /// <value>
        ///     The accessed at.
        /// </value>
        public DateTime? AccessedAt { get; set; }

        #endregion
    }
}