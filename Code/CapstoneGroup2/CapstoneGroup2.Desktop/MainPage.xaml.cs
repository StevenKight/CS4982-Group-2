using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using CapstoneGroup2.Desktop.Model;
using CapstoneGroup2.Desktop.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Data members

        private readonly SourceViewModel _sourcesViewModel;

        private List<Source> _sources;

        #endregion

        #region Constructors

        public MainPage()
        {
            this._sourcesViewModel = new SourceViewModel();

            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    var sourcesEnumerable = await this._sourcesViewModel.GetSources();
                    this._sources = sourcesEnumerable.ToList();
                    this.navigationView.SelectedItem = this.navigationView.MenuItems[0];
                });

            this.InitializeComponent();

            var size = new Size(Width, Height);
            ApplicationView.PreferredLaunchViewSize = size;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            ApplicationView.GetForCurrentView().SetPreferredMinSize(size);
            ApplicationView.GetForCurrentView().TryResizeView(size);
        }

        #endregion

        #region Methods

        private void navigationView_SelectionChanged(NavigationView sender,
            NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;
            var selectedItemContent = selectedItem.Content;

            switch (selectedItemContent)
            {
                case "Dashboard":
                    this.contentFrame.Navigate(typeof(DashboardPage), this._sources); // TODO: Add the shared sources
                    break;
                case "My Sources":
                    this.contentFrame.Navigate(typeof(MySourcesPage), this._sources);
                    break;
                case "Shared With Me":
                    this.contentFrame.Navigate(typeof(SharedPage)); // TODO: Add the shared sources
                    break;
                case "Settings":
                    this.contentFrame.Navigate(typeof(SettingsPage));
                    break;
            }
        }

        #endregion
    }
}