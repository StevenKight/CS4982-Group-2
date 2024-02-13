using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CapstoneGroup2.Desktop.Model;
using Windows.Storage.Pickers;
using System.Threading.Tasks;
using System;
using CapstoneGroup2.Desktop.Dal;
using CapstoneGroup2.Desktop.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page
    {
        #region Data members

        private List<Source> _sources;

        private SourceViewModel _sourceViewModel;

        #endregion

        #region Constructors

        public DashboardPage()
        {
            this.InitializeComponent();

            this._sourceViewModel = new SourceViewModel();

            var size = new Size(Width, Height);
            ApplicationView.PreferredLaunchViewSize = size;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            ApplicationView.GetForCurrentView().SetPreferredMinSize(size);
            ApplicationView.GetForCurrentView().TryResizeView(size);
        }

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this._sources = (List<Source>)e.Parameter;
            this.sourcesListBox.ItemsSource = this._sources.Take(4); // TODO: Order by date
        }

        #endregion

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
    }
}