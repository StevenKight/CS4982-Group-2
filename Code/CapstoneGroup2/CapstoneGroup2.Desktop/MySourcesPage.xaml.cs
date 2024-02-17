using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CapstoneGroup2.Desktop.Model;
using Windows.Storage.Pickers;
using CapstoneGroup2.Desktop.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MySourcesPage : Page
    {
        #region Data members

        private List<Source> _sources;
        private SourceViewModel _sourceViewModel;

        #endregion

        #region Constructors

        public MySourcesPage()
        {
            this.InitializeComponent();

            var size = new Size(Width, Height);
            ApplicationView.PreferredLaunchViewSize = size;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            ApplicationView.GetForCurrentView().SetPreferredMinSize(size);
            ApplicationView.GetForCurrentView().TryResizeView(size);
            this._sourceViewModel = new SourceViewModel();
        }

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this._sources = (List<Source>)e.Parameter;
            this.sourcesListBox.ItemsSource = this._sources;
        }

        private void sharedSourcesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(NotesPage), this._sources[this.sourcesListBox.SelectedIndex]);
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".pdf");
            var file = await picker.PickSingleFileAsync();

            await this._sourceViewModel.addNewSource(file);
        }

        #endregion
    }
}