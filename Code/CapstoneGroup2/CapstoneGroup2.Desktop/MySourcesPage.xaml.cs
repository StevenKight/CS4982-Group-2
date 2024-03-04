using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CapstoneGroup2.Desktop.Library.Model;
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

        private readonly SourceViewModel _sourceViewModel;

        #endregion

        #region Constructors

        public MySourcesPage()
        {
            this.InitializeComponent();

            this._sourceViewModel = new SourceViewModel();
        }

        #endregion

        #region Methods

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await this.loadSources();
        }

        private async Task loadSources()
        {
            var sourcesEnumerable = await this._sourceViewModel.GetSources(); // TODO: Add the shared sources
            this._sources = sourcesEnumerable.ToList();

            this.sourcesListBox.ItemsSource = this._sources; // TODO: Order by date
        }

        private void sharedSourcesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(NotesPage), this._sources[this.sourcesListBox.SelectedIndex]);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddSourceDialog();
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var source = dialog.NewSource;
                await this._sourceViewModel.addNewSource(source);

                await this.loadSources();
            }
        }

        #endregion
    }
}