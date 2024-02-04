﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Display.Core;
using Desktop_Client.Dal;
using Desktop_Client.Model;

namespace Desktop_Client.ViewModel
{
    /// <summary>
    /// ViewModel for Docunotes that handles data access methods
    /// </summary>
    public class ViewModel
    {
        /// <summary>
        /// Gets or sets the recent notes.
        /// </summary>
        /// <value>
        /// The recent notes.
        /// </value>
        public List<Note> RecentNotes { get; set; }

        /// <summary>
        /// Gets or sets the user notes.
        /// </summary>
        /// <value>
        /// The user notes.
        /// </value>
        public List<Note> UserNotes { get; set; }

        /// <summary>
        /// Gets or sets the recent notes source.
        /// </summary>
        /// <value>
        /// The recent notes source.
        /// </value>
        public List<Source> recentNotesSource { get; set; }

        /// <summary>
        /// Gets or sets the user notes source.
        /// </summary>
        /// <value>
        /// The user notes source.
        /// </value>
        public List<Source> userNotesSource { get; set; }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        public User CurrentUser { get; set; }

        /// <summary>
        /// Gets or sets the current source.
        /// </summary>
        /// <value>
        /// The current source.
        /// </value>
        public Source CurrentSource { get; set; }

        /// <summary>
        /// The token
        /// </summary>
        private string token;

        /// <summary>
        /// The notes dal
        /// </summary>
        private NotesDal notesDal;
        /// <summary>
        /// The source dal
        /// </summary>
        private SourceDal sourceDal;
        /// <summary>
        /// The user dal
        /// </summary>
        private UserDal userDal;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            this.CurrentUser = new User();
            this.RecentNotes = new List<Note>();
            this.UserNotes = new List<Note>();
            this.recentNotesSource = new List<Source>();
            this.userNotesSource = new List<Source>();
            this.CurrentSource = new Source();
            this.token = "";
            this.notesDal = new NotesDal();
            this.sourceDal = new SourceDal();
            this.userDal = new UserDal();
        }

        /// <summary>
        /// Creates the new note.
        /// </summary>
        /// <param name="noteText">The note text.</param>
        /// <returns></returns>
        public async Task<bool> CreateNewNote(string noteText)
        {
            var newNote = new Note();
            newNote.Source = CurrentSource;
            newNote.Username = CurrentUser.Username;
            newNote.NoteText = noteText;
            newNote.TagsString = "";
            newNote.SourceId = CurrentSource.SourceId;
            this.UserNotes.Add(newNote);
            
            return await this.notesDal.createNewNote(newNote,CurrentUser.Token);
        }

        /// <summary>
        /// Updates the note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        public async Task<bool> updateNote (Note note)
        {
            if (note != null)
            {
                try
                {
                    return await this.notesDal.UpdateNote(note, CurrentUser.Token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating note: {ex.Message}");
                    return false;
                }
            }
            else
            {
                return false;
            }
                
        }

        /// <summary>
        /// Deletes the note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        public async Task<bool>  DeleteNote(Note note)
        {
            if (note != null)
            {
                try
                {
                    return await this.notesDal.DeleteNote(note, CurrentUser.Token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting note: {ex.Message}");
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<bool> Login(string username, string password)
        {
            var result = await this.userDal.Login(username, password);
            if (result != null)
            {
                this.CurrentUser = result;
                return true;
            }
            else
            {
                return false;
            }
           
        }

        /// <summary>
        /// Gets the sources for user.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Source>> getSourcesForUser()
        {
            return await this.sourceDal.getSourcesForUser(this.CurrentUser);
        }

        /// <summary>
        /// Gets the sources.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Source>> getSources()
        {
            return await this.sourceDal.getSources();
        }
    }
}
