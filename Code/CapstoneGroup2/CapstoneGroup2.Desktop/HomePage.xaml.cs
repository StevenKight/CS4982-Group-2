using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CapstoneGroup2.Desktop.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        #region Data members

        private readonly ObservableCollection<Source> sources;
        private ViewModel.ViewModel _userViewModel;

        #endregion

        #region Constructors

        public HomePage()
        {
            this.InitializeComponent();
            this.sources = new ObservableCollection<Source>();
            var source1 = new Source();
            source1.IsLink = false;
            source1.Link = "";
            source1.Name = "Test Source";
            source1.SourceId = 1;
            source1.Type = "Pdf";
            var source2 = new Source();
            source2.IsLink = false;
            source2.Link = "";
            source2.Name = "Test Source";
            source2.SourceId = 1;
            source2.Type = "Pdf";
            this.sources.Add(source1);
            this.sources.Add(source2);
            this.RecentNotesListView.ItemsSource = this.sources;
        }

        #endregion

        #region Methods

        private void RecentNotesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle selection changes if needed
            var selectedItem = e.AddedItems.FirstOrDefault();

            if (selectedItem != null)
            {
                var source = (Source)selectedItem;
                this._userViewModel.CurrentSource = source;
                Frame.Navigate(typeof(MainPage), this._userViewModel);
            }
        }

        private async void PickAndSendPdfButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a file picker to select a PDF file
            var filePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            filePicker.FileTypeFilter.Add(".pdf");

            var pickedFile = await filePicker.PickSingleFileAsync();
            if (pickedFile != null)
            {
                var pdfBytes = await this.ReadFileAsync(pickedFile);

                //await SendPdfToServer(pdfBytes);
            }
        }

        private async Task<byte[]> ReadFileAsync(StorageFile file)
        {
            using (var stream = await file.OpenReadAsync())
            {
                using (var dataReader = new DataReader(stream))
                {
                    await dataReader.LoadAsync((uint)stream.Size);
                    var fileBytes = new byte[stream.Size];
                    dataReader.ReadBytes(fileBytes);
                    return fileBytes;
                }
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

        #endregion
    }
}