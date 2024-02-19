using Windows.UI.Xaml.Controls;
using CapstoneGroup2.Desktop.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        #region Data members

        private readonly UserViewModel _viewModel;

        #endregion

        #region Constructors

        public SettingsPage()
        {
            this._viewModel = new UserViewModel();
            this.InitializeComponent();
        }

        #endregion
    }
}