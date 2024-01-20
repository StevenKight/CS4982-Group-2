using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Desktop_Client.Model;

namespace Desktop_Client.Dal
{
    public class WeatherDal
    {
        #region Data members

        private static readonly HttpClient client = new HttpClient();

        private static readonly string baseUrl = "https://localhost:7041/";

        #endregion

        #region Methods

        public static async Task<List<WeatherForecast>> GetWeatherForecastsAsync()
        {
            client.BaseAddress = new Uri(baseUrl);

            var weatherForecasts = new List<WeatherForecast>();
            var response = await client.GetAsync("WeatherForecast");
            if (response.IsSuccessStatusCode)
            {
                weatherForecasts = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();
            }

            return weatherForecasts;
        }

        #endregion
    }
}