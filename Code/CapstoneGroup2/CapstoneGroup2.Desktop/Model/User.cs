namespace CapstoneGroup2.Desktop.Model
{
    /// <summary>
    /// The User model class
    /// </summary>
    public class User
    {
        #region Properties

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string? Token { get; set; }

        #endregion
    }
}