using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CapstoneGroup2.Desktop.Model;

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
        }

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this._sources = (List<Source>)e.Parameter;
            this.sourcesListBox.ItemsSource = this._sources;
        }

        #endregion
    }
}