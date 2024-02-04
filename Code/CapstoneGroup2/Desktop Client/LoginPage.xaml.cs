using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Desktop_Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private ViewModel.ViewModel _viewModel;
        private string _username;
        private string _password;
        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    this.UsernameTextBox.Text = value;
                }
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    this.PasswordTextBox.Text = value;
                }
            }
        }
        public LoginPage()
        {
            this._viewModel = new ViewModel.ViewModel();
            this.InitializeComponent();
        }

        private async void Login()
        {
            //if (await this._viewModel.Login(this.Username, this.Password))
            //{
            //    Frame.Navigate(typeof(HomePage), this._viewModel);
            //}
            Frame.Navigate(typeof(HomePage), this._viewModel);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.Login();
        }
    }
}
