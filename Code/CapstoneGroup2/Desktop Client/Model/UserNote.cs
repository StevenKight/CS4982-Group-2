namespace Desktop_Client.Model
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

        public string ObjectLink { get; set; }

        public NoteType NoteType { get; set; }

        #endregion
    }
}