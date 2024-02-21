using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CapstoneGroup2.Desktop.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        #region Data members

        private readonly UserViewModel _viewModel;

        #endregion

        #region Properties

        public string Username
        {
            get => this.usernameTextBox.Text;
            set => this.usernameTextBox.Text = value;
        }

        public string Password
        {
            get => this.passwordTextBox.Password;
            set => this.passwordTextBox.Password = value;
        }

        #endregion

        #region Constructors

        public LoginPage()
        {
            this._viewModel = new UserViewModel();
            this.InitializeComponent();

            var size = new Size(Width, Height);
            ApplicationView.PreferredLaunchViewSize = size;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            ApplicationView.GetForCurrentView().SetPreferredMinSize(size);
            ApplicationView.GetForCurrentView().TryResizeView(size);
        }

        #endregion

        #region Methods

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await this._viewModel.Login(this.Username, this.Password);

            if (this._viewModel.ValidateAuthorization())
            {
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                this.errorTextBlock.Text = "Invalid username or password";
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SignUpPage));
        }

        #endregion
    }
}