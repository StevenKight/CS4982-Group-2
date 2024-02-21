using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CapstoneGroup2.Desktop.Library.Model;
using CapstoneGroup2.Desktop.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUpPage : Page
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

        public string ConfirmPassword
        {
            get => this.confirmPasswordTextBox.Password;
            set => this.confirmPasswordTextBox.Password = value;
        }

        #endregion

        #region Constructors

        public SignUpPage()
        {
            this._viewModel = new UserViewModel();

            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private async void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Password.Equals(this.ConfirmPassword))
            {
                Console.WriteLine("Passwords do not match");
                return;
            }

            var user = new User
            {
                Username = this.Username,
                Password = this.Password
            };

            await this._viewModel.CreateAccount(user);

            if (this._viewModel.ValidateAuthorization())
            {
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                this.errorTextBlock.Text = "Invalid username or password";
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        #endregion
    }
}