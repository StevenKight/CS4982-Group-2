using System;

namespace Desktop_Client.Model
{
    public class WeatherForecast
    {
        #region Properties

        public DateTime Date { get; set; }

        public decimal TemperatureC { get; set; }

        public decimal TemperatureF => 32 + (int)(this.TemperatureC / (decimal)0.5556);

        public string Summary { get; set; }

        #endregion
    }
}