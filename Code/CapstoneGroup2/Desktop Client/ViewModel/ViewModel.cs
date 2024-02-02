using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Desktop_Client.Model;

namespace Desktop_Client.ViewModel
{
    public class ViewModel
    {
        public List<Note> RecentNotes { get; set; }

        public List<Note> UserNotes { get; set; }

        public List<Source> recentNotesSource { get; set; }

        public List<Source> userNotesSource { get; set; }

        public User CurrentUser { get; set; }

        public Source CurrentSource { get; set; }

        public ViewModel()
        {
            this.CurrentUser = new User();
            this.RecentNotes = new List<Note>();
            this.UserNotes = new List<Note>();
            this.recentNotesSource = new List<Source>();
            this.userNotesSource = new List<Source>();
            this.CurrentSource = new Source();

            this.LoadFromDB();
        }

        private void LoadFromDB()
        {
            //Make all calls to initialize members with the db;
        }

        public void RefreshInfoFromDb()
        {
            //Make all calls to refresh members from the db
            this.LoadFromDB();
        }



        public bool CreateNewNote(Note note)
        {
            //Db call to upload the note
            this.UserNotes.Add(note);
            return false;
        }

        public bool Login(string username, string password)
        {
            //Make call to db to login user
            return true;
        }
    }
}
