using System;
using System.Collections.Generic;
using System.Text;

namespace CapstoneGroup2.Desktop.Library.Model
{
    public class Tag
    {
        /// <summary>
        /// Gets or sets the tag identifier.
        /// </summary>
        /// <value>
        /// The tag identifier.
        /// </value>
        public int TagID { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <value>
        /// The name of the tag.
        /// </value>
        public string TagName { get; set; }

        /// <summary>
        /// Returns the name of the tag.
        /// </summary>
        /// <returns>The name of the tag.</returns>
        public override string ToString()
        {
            return TagName;
        }
    }
}
