using CapstoneGroup2.Desktop.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Pdf;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using CapstoneGroup2.Desktop.Data;
using System.Net;
using Windows.UI.ViewManagement;

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    /// Notes Page for taking notes on a source
    /// </summary>
    public sealed partial class NotesPage : Page
    {
        #region Data members

        private PdfDocument pdfDocument;
        private ViewModel.NotesViewModel _notesViewModel;
        private ViewModel.SourceViewModel _sourceViewModel;

        private List<Note> Notes;

        private int currentPageNumber;

        #endregion
        public NotesPage()
        {
            this._notesViewModel = new ViewModel.NotesViewModel();
            this._sourceViewModel = new ViewModel.SourceViewModel();
            this.InitializeComponent();
            
        }

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Source source)
            {
                var view = ApplicationView.GetForCurrentView();
                view.TryEnterFullScreenMode();
                this._sourceViewModel.currentSource = source;
                try
                {
                    DataManager.SaveVarBinaryAsPdf(this._sourceViewModel.currentSource.Content, "./currentSource.pdf");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
                this.setSource();
                this.LoadNotes();
            }
        }

        private async void setSource()
        {
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile storageFile =
                    await StorageFile.GetFileFromPathAsync(Path.Combine(localFolder.Path, "current.pdf"));
                this.GatherDocument(storageFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private async void LoadNotes()
        {
            try
            {
                this.progressControl.Visibility = Visibility.Visible;
                this.Notes =  await this._notesViewModel.GetSourceNotes(this._sourceViewModel.currentSource) as List<Note>;
                var emptyNote = new Note();
                emptyNote.Username = "";
                emptyNote.NoteText = "";
                this.Notes.Add(emptyNote);
                for (int i = 0; i < this.Notes.Count; i++)
                {
                    var note = this.Notes[i];

                    var textBox = new TextBox();
                    textBox.MaxWidth = 200;
                    textBox.Width = 190;
                    textBox.TextWrapping = TextWrapping.Wrap;
                    textBox.Name = $"noteTextBox{i}";
                    textBox.Text = note.NoteText;

                    textBox.Tag = note;

                    textBox.KeyDown += NoteTextBox_KeyDown;

                    this.notesListBox.Items?.Add(textBox);
                }

                this.progressControl.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void NoteTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                var textBox = (TextBox)sender;
                textBox.IsEnabled = false;
                textBox.IsEnabled = true;
                var correspondingNote = (Note)textBox.Tag;

                var updatedText = textBox.Text;

                correspondingNote.NoteText = updatedText;
                if (correspondingNote.NoteText.Equals("") && (Notes.IndexOf(correspondingNote) < Notes.Count - 1))
                {
                    var deleteIndex = this.Notes.IndexOf(correspondingNote);
                    this.Notes.Remove(correspondingNote);
                    var itemCollection = this.notesListBox.Items;
                    if (itemCollection != null) itemCollection.RemoveAt(deleteIndex);
                    this.DeleteNote(correspondingNote);
                }
                else if (correspondingNote.Username == "")
                {
                    var emptyNote = new Note();
                    emptyNote.NoteText = "";
                    emptyNote.Username = "";
                    this.Notes.Add(emptyNote);
                    correspondingNote.SourceId = this._sourceViewModel.currentSource.SourceId;
                    this.AddNewNote(correspondingNote);

                    var newTextbox = new TextBox();
                    newTextbox.MaxWidth = 200;
                    newTextbox.Width = 190;
                    newTextbox.TextWrapping = TextWrapping.Wrap;
                    newTextbox.Name = $"noteTextBox{this.Notes.Count-1}";
                    newTextbox.Text = emptyNote.NoteText;
                    newTextbox.Tag = emptyNote;
                    newTextbox.KeyDown += NoteTextBox_KeyDown;
                    this.notesListBox.Items?.Add(newTextbox);

                }

                this.UpdateNote(correspondingNote);
            }
        }

        private async void AddNewNote(Note note)
        {
            await this._notesViewModel.AddNewNote(note);
        }

        private async void DeleteNote(Note note)
        {
            await this._notesViewModel.DeleteNote(note);
        }

        private async void UpdateNote(Note note)
        {
            await this._notesViewModel.updateNote(note);
        }

        private void notesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedNote = (Note)this.notesListBox.SelectedItem;

            if (selectedNote == null)
            {
                return;
            }

            this.progressControl.Visibility = Visibility.Visible;

        }

        private async void GatherDocument(IStorageFile file)
        {
            if (file != null)
            {
                this.pdfDocument = null;
                this.objectRender.Source = null;

                try
                {
                    this.pdfDocument = await PdfDocument.LoadFromFileAsync(file);
                    this.currentPageNumber = 0;
                    this.UpdatePage();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                if (this.pdfDocument != null)
                {
                    if (this.pdfDocument.IsPasswordProtected)
                    {
                        Console.WriteLine("Pdf is Password Protected");
                    }
                }
            }
        }

        private async void UpdatePage()
        {
            this.objectRender.Source = null;
            this.progressControl.Visibility = Visibility.Visible;

            var pageIndex = this.currentPageNumber;

            using (var page = this.pdfDocument.GetPage((uint)pageIndex))
            {
                var stream = new InMemoryRandomAccessStream();

                var options1 = new PdfPageRenderOptions
                {
                    DestinationHeight = (uint)this.objectRender.Height,
                    DestinationWidth = (uint)this.objectRender.Width,
                };

                await page.RenderToStreamAsync(stream, options1);

                var src = new BitmapImage();
                this.objectRender.Source = src;
                await src.SetSourceAsync(stream);
            }

            this.progressControl.Visibility = Visibility.Collapsed;
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentPageNumber > 0)
            {
                this.currentPageNumber--;
                this.UpdatePage();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (this.pdfDocument != null && this.currentPageNumber < this.pdfDocument.PageCount - 1)
            {
                this.currentPageNumber++;
                this.UpdatePage();
            }
        }

        #endregion

        //private void loadVideo(object sender, RoutedEventArgs args)
        //{
        //    this.documentsListView.SelectedItem = null;

        //    this.webPlayer.Navigate(new Uri("about:blank"));
        //    this.mediaPlayer.Source = null;

        //    this.documentOptions.Visibility = Visibility.Collapsed;
        //    this.videoOptions.Visibility = Visibility.Visible;

        //    this.documentDisplay.Visibility = Visibility.Collapsed;
        //    this.videoDisplay.Visibility = Visibility.Visible;
        //}

        //private void renderVideo(object sender, RoutedEventArgs args)
        //{
        //    this.documentDisplay.Visibility = Visibility.Collapsed;
        //    this.videoDisplay.Visibility = Visibility.Visible;

        //    var videoUrl = this.videoUrl.Text;

        //    if (string.IsNullOrEmpty(videoUrl))
        //    {
        //        Console.WriteLine("Please enter a video url.");
        //        return;
        //    }

        //    if (!Uri.TryCreate(videoUrl, UriKind.Absolute, out var uri))
        //    {
        //        Console.WriteLine("Please enter a valid video url.");
        //        return;
        //    }

        //    this.progressControl.Visibility = Visibility.Visible;

        //    var fileExtension = Path.GetExtension(videoUrl);

        //    var supportedExtensions = new List<string>
        //    {
        //        ".avi", // Raw Video (RGBA format)
        //        ".yuv", // YV12 format
        //        ".wmv", // Windows Media Video formats
        //        ".wma", // Windows Media Audio formats
        //        ".h264", // H.264 format
        //        ".h263", // H.263 format
        //        ".mp4" // MPEG-4 Part 2 format
        //    };

        //    if (supportedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
        //    {
        //        this.webPlayer.Navigate(new Uri("about:blank"));
        //        this.webPlayer.Visibility = Visibility.Collapsed;

        //        this.mediaPlayer.Visibility = Visibility.Visible;
        //        this.mediaPlayer.Source = MediaSource.CreateFromUri(uri);
        //    }
        //    else
        //    {
        //        this.mediaPlayer.Source = null;
        //        this.mediaPlayer.Visibility = Visibility.Collapsed;

        //        this.webPlayer.Visibility = Visibility.Visible;
        //        this.webPlayer.Source = uri;
        //    }

        //    this.progressControl.Visibility = Visibility.Collapsed;
        //}
    }
}
