﻿using System;
using System.Text.RegularExpressions;
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
            this.requirementsToolTip.Content = "Password needs 8 characters with one letter, number, and special character";
            this.passwordTextBox.PasswordChanged += passwordTextBox_PasswordChanged;
        }

        #endregion

        #region Methods

        private async void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.usernameToolTip.Visibility = Visibility.Collapsed;
            this.passwordToolTip.Visibility = Visibility.Collapsed;
            this.confirmPasswordToolTip.Visibility = Visibility.Collapsed;

            var invalid = !this.blankCheck() | !this.CheckConfirmationPassword();

            if (invalid)
            {
                return;
            }

            var user = new User
            {
                Username = this.Username,
                Password = this.Password
            };

            try
            {
                await this._viewModel.CreateAccount(user);
                this.usernameToolTip.Visibility = Visibility.Collapsed;

                if (this._viewModel.ValidateAuthorization())
                {
                    Frame.Navigate(typeof(MainPage));
                }
            }
            catch (Exception exception)
            {
                this.usernameToolTip.Visibility = Visibility.Visible;
                this.usernameToolTip.Content = exception.Message;
            }
        }

        private bool CheckConfirmationPassword()
        {
            if (!this.Password.Equals(this.ConfirmPassword))
            {
                this.confirmPasswordToolTip.Visibility = Visibility.Visible;

                if (this.passwordToolTip.Visibility == Visibility.Visible)
                {
                    this.confirmPasswordToolTip.Margin = new Thickness(375, 405, 0, 0);
                }
                else
                {
                    this.confirmPasswordToolTip.Margin = new Thickness(375, 375, 0, 0);
                }

                if (this.passwordToolTip.Visibility != Visibility.Visible)
                {
                    this.confirmPasswordToolTip.Content = "Passwords do not match";
                }
                return false;
            }

            return true;
        }

        private void passwordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            const string passwordRegex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";
            if (!string.IsNullOrEmpty(this.passwordTextBox.Password) && Regex.IsMatch(this.passwordTextBox.Password, passwordRegex))
            {
                this.requirementsToolTip.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.requirementsToolTip.Visibility = Visibility.Visible;
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

            const string passwordRegex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";
            if (string.IsNullOrEmpty(this.Password))
            {
                this.passwordToolTip.Visibility = Visibility.Visible;
                this.requirementsToolTip.Visibility = Visibility.Collapsed;
                this.passwordToolTip.Content = "Password cannot be empty";
                valid = false;
            }
            else if (!Regex.IsMatch(this.Password, passwordRegex))
            {
                this.passwordToolTip.Visibility = Visibility.Visible;
                this.requirementsToolTip.Visibility = Visibility.Visible;
                this.passwordToolTip.Content =
                    "Password must contain at least 8 characters, one letter, one number and one special character";
                valid = false;
            }
            else
            {
                this.requirementsToolTip.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(this.ConfirmPassword))
            {
                this.confirmPasswordToolTip.Visibility = Visibility.Visible;
                this.confirmPasswordToolTip.Content = "Password cannot be empty";
                valid = false;
            }

            return valid;
        }


        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        #endregion
    }
}