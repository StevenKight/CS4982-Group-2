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

        private ObservableCollection<Source> sources;
        private ViewModel.ViewModel _viewModel;

        #endregion

        #region Constructors

        public HomePage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ViewModel.ViewModel userViewModel)
            {
                this._viewModel = userViewModel;

                this.LoadData();
            }
        }

        private async void LoadData()
        {
            this.sources = new ObservableCollection<Source>();

            var sources = await this._viewModel.getSourcesForUser();

            foreach (var source in sources)
            {
                this.sources.Add(source);
            }

            this.RecentNotesListView.ItemsSource = this.sources;
        }

        private void RecentNotesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle selection changes if needed
            var selectedItem = e.AddedItems.FirstOrDefault();

            if (selectedItem != null)
            {
                var source = (Source)selectedItem;
                this._viewModel.CurrentSource = source;
                Frame.Navigate(typeof(MainPage), this._viewModel);
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

        #endregion
    }
}