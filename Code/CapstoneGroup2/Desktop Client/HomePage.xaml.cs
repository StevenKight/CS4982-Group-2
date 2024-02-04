using Desktop_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Desktop_Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private ObservableCollection<Source> sources;
        private ViewModel.ViewModel _userViewModel;

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
            sources.Add(source1);
            sources.Add(source2);
            this.RecentNotesListView.ItemsSource = this.sources;
        }

        private void RecentNotesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle selection changes if needed
            object selectedItem = e.AddedItems.FirstOrDefault();

            if (selectedItem != null)
            {
                Source source = (Source)selectedItem;
                _userViewModel.CurrentSource = source;
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

            StorageFile pickedFile = await filePicker.PickSingleFileAsync();
            if (pickedFile != null)
            {
                byte[] pdfBytes = await ReadFileAsync(pickedFile);

                //await SendPdfToServer(pdfBytes);
            }
        }

        private async Task<byte[]> ReadFileAsync(StorageFile file)
        {
            using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
            {
                using (DataReader dataReader = new DataReader(stream))
                {
                    await dataReader.LoadAsync((uint)stream.Size);
                    byte[] fileBytes = new byte[stream.Size];
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
                _userViewModel = userViewModel;

            }
        }
    }
}
