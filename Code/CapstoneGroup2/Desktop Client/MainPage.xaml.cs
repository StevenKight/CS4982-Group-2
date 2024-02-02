﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Data.Pdf;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Desktop_Client.Dal;
using Desktop_Client.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Desktop_Client
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Data members

        private PdfDocument pdfDocument;
        private ViewModel.ViewModel _userViewModel;

        private List<UserNote> userNotes;

        private List<Note> Notes;

        private int currentPageNumber;

        #endregion

        #region Constructors

        public MainPage()
        {
            this.InitializeComponent();
            this.Notes = new List<Note>();
            var note = new Note();
            note.Username = "Chimbus";
            note.SourceId = 3;
            note.NoteText = "Wubba";
            this.Notes.Add(note);
            this.currentPageNumber = 0;
            //this.loadNotes();
        }

        #endregion

        #region Methods

        //private async void loadNotes()
        //{
        //    this.progressControl.Visibility = Visibility.Visible;

        //    this.userNotes = await NotesDal.GetUsersNotesAsync();
        //    this.documentsListView.ItemsSource = this.userNotes;

        //    this.progressControl.Visibility = Visibility.Collapsed;
        //}

        //private void documentsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selectedNote = (UserNote)this.documentsListView.SelectedItem;

        //    if (selectedNote == null)
        //    {
        //        return;
        //    }

        //    this.documentOptions.Visibility = Visibility.Collapsed;
        //    this.videoOptions.Visibility = Visibility.Collapsed;

        //    this.progressControl.Visibility = Visibility.Visible;

        //    switch (selectedNote.NoteType)
        //    {
        //        case NoteType.Pdf:
        //            Console.WriteLine("Pdf");
        //            break;
        //        case NoteType.Vid:
        //            this.videoUrl.Text = selectedNote.ObjectLink;
        //            this.renderVideo(sender, e);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        private async void loadDocument(object sender, RoutedEventArgs args)
        {

            this.progressControl.Visibility = Visibility.Visible;

            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".pdf");
            var file = await picker.PickSingleFileAsync();

            this.gatherDocument(file);

            this.progressControl.Visibility = Visibility.Collapsed;
        }

        private async void gatherDocument(IStorageFile file)
        {
            if (file != null)
            {
                this.pdfDocument = null;
                this.objectRender.Source = null;

                try
                {
                    this.pdfDocument = await PdfDocument.LoadFromFileAsync(file);
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

                //this.pageSelector.SelectedIndex = 0;
                //this.documentOptions.Visibility = Visibility.Visible;
                //this.documentDisplay.Visibility = Visibility.Visible;

                //this.viewPage(null, null);
            }
        }

            private async void viewPage(object sender, SelectionChangedEventArgs e)
            {
                //rootPage.NotifyUser("", NotifyType.StatusMessage);


                this.objectRender.Source = null;
                this.progressControl.Visibility = Visibility.Visible;

               
                var pageIndex = this.currentPageNumber - 1;

                using (var page = this.pdfDocument.GetPage((uint)pageIndex))
                {
                    var stream = new InMemoryRandomAccessStream();

                    var options1 = new PdfPageRenderOptions
                    {
                        DestinationHeight = (uint)this.objectRender.Height
                    };

                    await page.RenderToStreamAsync(stream, options1);

                    var src = new BitmapImage();
                    this.objectRender.Source = src;
                    await src.SetSourceAsync(stream);
                }

                this.progressControl.Visibility = Visibility.Collapsed;
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
                    DestinationHeight = (uint)this.objectRender.Height
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
                if (currentPageNumber > 0)
                {
                    this.currentPageNumber--;
                    this.UpdatePage();
                }
            }

            private void NextPage_Click(object sender, RoutedEventArgs e)
            {
                if (pdfDocument != null && this.currentPageNumber < pdfDocument.PageCount - 1)
                {
                    this.currentPageNumber++;
                    this.UpdatePage();
                }
            }
            protected override void OnNavigatedTo(NavigationEventArgs e)
            {
                base.OnNavigatedTo(e);

                if (e.Parameter is ViewModel.ViewModel userViewModel)
                {
                    this._userViewModel = userViewModel;

                }
            }

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

        #endregion
    }
}