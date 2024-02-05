using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        #region Data members

        private readonly ViewModel.ViewModel _viewModel;

        #endregion

        #region Properties

        public string Username
        {
            get => this.UsernameTextBox.Text;
            set => this.UsernameTextBox.Text = value;
        }

        public string Password
        {
            get => this.PasswordTextBox.Text;
            set => this.PasswordTextBox.Text = value;
        }

        #endregion

        #region Constructors

        public LoginPage()
        {
            this._viewModel = new ViewModel.ViewModel();
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private async void Login()
        {
            await this._viewModel.Login(this.Username, this.Password);

            if (this._viewModel.CurrentUser != null)
            {
                Frame.Navigate(typeof(HomePage), this._viewModel);
            }
            else
            {
                this.ErrorTextBlock.Text = "Invalid username or password";
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.Login();
        }

        #endregion
    }
}