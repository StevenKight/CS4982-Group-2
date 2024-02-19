using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Constructors

        public MainPage()
        {
            this.InitializeComponent();

            var size = new Size(Width, Height);
            ApplicationView.PreferredLaunchViewSize = size;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            ApplicationView.GetForCurrentView().SetPreferredMinSize(size);
            ApplicationView.GetForCurrentView().TryResizeView(size);

            this.navigationView.SelectedItem = this.navigationView.MenuItems[0];
        }

        #endregion

        #region Methods

        private void navigationView_SelectionChanged(NavigationView sender,
            NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;

            if (selectedItem == null)
            {
                return;
            }

            var selectedItemContent = selectedItem.Content;

            switch (selectedItemContent)
            {
                case "Dashboard":
                    this.contentFrame.Navigate(typeof(DashboardPage)); // TODO: Add the shared sources
                    break;
                case "My Sources":
                    this.contentFrame.Navigate(typeof(MySourcesPage));
                    break;
                case "Shared With Me":
                    this.contentFrame.Navigate(typeof(SharedPage)); // TODO: Add the shared sources
                    break;
                case "Settings":
                    this.contentFrame.Navigate(typeof(SettingsPage));
                    break;
            }
        }

        private void contentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            this.navigationView.SelectedItem = e.SourcePageType switch
            {
                { } when e.SourcePageType == typeof(DashboardPage) => this.navigationView.MenuItems[0],
                { } when e.SourcePageType == typeof(MySourcesPage) => this.navigationView.MenuItems[1],
                { } when e.SourcePageType == typeof(SharedPage) => this.navigationView.MenuItems[2],
                { } when e.SourcePageType == typeof(SettingsPage) => this.navigationView.SelectedItem,
                _ => null
            };
        }

        #endregion
    }
}