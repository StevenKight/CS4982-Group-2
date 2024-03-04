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
            this.usernameToolTip.Visibility = Visibility.Collapsed;
            this.passwordToolTip.Visibility = Visibility.Collapsed;

            if (!this.blankCheck())
            {
                return;
            }

            await this._viewModel.Login(this.Username, this.Password);

            if (this._viewModel.ValidateAuthorization())
            {
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                this.usernameToolTip.Visibility = Visibility.Visible;
                this.usernameToolTip.Content = "Invalid username or password";
            }
        }

        private bool blankCheck()
        {
            var valid = true;
            if (string.IsNullOrEmpty(this.Username))
            {
                this.usernameToolTip.Visibility = Visibility.Visible;
                this.usernameToolTip.Content = "Username cannot be empty";
                valid = false;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                this.passwordToolTip.Visibility = Visibility.Visible;
                this.passwordToolTip.Content = "Password cannot be empty";
                valid = false;
            }

            return valid;
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUpPage));
        }

        #endregion
    }
}