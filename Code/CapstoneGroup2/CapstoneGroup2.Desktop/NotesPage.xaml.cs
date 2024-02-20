using System;
using System.Collections.Generic;
using System.IO;
using Windows.Data.Pdf;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using CapstoneGroup2.Desktop.Data;
using CapstoneGroup2.Desktop.Library.Model;
using CapstoneGroup2.Desktop.ViewModel;

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     Notes Page for taking notes on a source
    /// </summary>
    public sealed partial class NotesPage : Page
    {
        #region Data members

        private PdfDocument pdfDocument;
        private readonly NotesViewModel _notesViewModel;
        private readonly SourceViewModel _sourceViewModel;

        private int currentPageNumber;

        #endregion

        #region Constructors

        public NotesPage()
        {
            this._notesViewModel = new NotesViewModel();
            this._sourceViewModel = new SourceViewModel();
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Source source)
            {
                this._sourceViewModel.currentSource = source;

                this.sourceHeading.Text = this._sourceViewModel.currentSource.Name;

                if (this._sourceViewModel.currentSource.IsLink)
                {
                    var fileUri = new Uri(this._sourceViewModel.currentSource.Link);

                    var destinationFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                        source.Name, CreationCollisionOption.GenerateUniqueName);

                    var downloader = new BackgroundDownloader();
                    var download = downloader.CreateDownload(fileUri, destinationFile);

                    await download.StartAsync();

                    this._sourceViewModel.currentSource.Content =
                        await DataManager.FileToBinary(destinationFile);
                }

                DataManager.SaveVarBinaryAsPdf(this._sourceViewModel.currentSource.Content, "./currentSource.pdf");

                this.setSource();
                this.LoadNotes();
            }
        }

        private async void setSource()
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var storageFile =
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
                var notes =
                    await this._notesViewModel.GetSourceNotes(this._sourceViewModel.currentSource) as List<Note>;

                this.notesListView.Items?.Clear();

                foreach (var note in notes)
                {
                    this.notesListView.Items?.Add(note);
                }

                this.progressControl.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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

                    if (this.pdfDocument.PageCount <= 1)
                    {
                        this.nextPageButton.IsEnabled = false;
                        this.previousPageButton.IsEnabled = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                if (this.pdfDocument is { IsPasswordProtected: true })
                {
                    Console.WriteLine("Pdf is Password Protected");
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
                    DestinationWidth = (uint)this.objectRender.Width
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

        private void notesListBox_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((ListView)sender);
        }

        private async void AddNoteItem_Click(object sender, RoutedEventArgs e)
        {
            var note = new Note
            {
                NoteText = "Enter Note...",
                SourceId = this._sourceViewModel.currentSource.SourceId
            };

            await this._notesViewModel.AddNewNote(note);

            this.LoadNotes();
        }

        private async void RemoveNoteItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItemIndex = this.notesListView.SelectedIndex;
            var selectedItem = (Note)this.notesListView.SelectedItem;

            if (selectedItem == null)
            {
                var flyout = new Flyout
                {
                    Content = new TextBlock
                    {
                        Text = "Please select a note to delete"
                    }
                };

                flyout.ShowAt(this.notesListView);
            }

            if (selectedItemIndex != -1)
            {
                this.notesListView.Items?.RemoveAt(selectedItemIndex);
            }

            await this._notesViewModel.DeleteNote(selectedItem);
        }

        private async void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var selectedItemTextBox = (Note)textBox.DataContext;
            selectedItemTextBox.NoteText = textBox.Text;

            if (string.IsNullOrEmpty(selectedItemTextBox.NoteText))
            {
                selectedItemTextBox.NoteText = "Enter Note...";
            }

            await this._notesViewModel.updateNote(selectedItemTextBox);

            textBox.MaxHeight = 30;

            var stackPanel = (Grid)textBox.Parent;
            var textBlock = (TextBlock)stackPanel.Children[1];
            textBlock.Visibility = Visibility.Visible;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var selectedItemTextBox = (Note)textBox.DataContext;

            this.notesListView.SelectedItem = selectedItemTextBox;

            textBox.MaxHeight = 100;

            var stackPanel = (Grid)textBox.Parent;
            var textBlock = (TextBlock)stackPanel.Children[1];
            textBlock.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}