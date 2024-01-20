using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Desktop_Client.Dal;
using Desktop_Client.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Desktop_Client
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Data members

        private List<WeatherForecast> weatherForecasts = new List<WeatherForecast>();

        #endregion

        #region Constructors

        public MainPage()
        {
            this.InitializeComponent();

            this.loadWeatherForecasts();
        }

        #endregion

        #region Methods

        private async void loadWeatherForecasts()
        {
            this.weatherForecasts = await WeatherDal.GetWeatherForecastsAsync();
            this.ForecastListView.ItemsSource = this.weatherForecasts;
        }

        #endregion
    }
}