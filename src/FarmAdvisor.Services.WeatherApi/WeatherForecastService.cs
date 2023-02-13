using Newtonsoft.Json;
using FarmAdvisor.Models;
using FarmAdvisor.Commons;
namespace FarmAdvisor.Services.WeatherApi
{
    public class ForecastApiResponse
    {
        public class Properties
        {
            public class TimeSeriesMember
            {
                public class TimeSeriesMemberData
                {
                    public class TimeSeriesMemberDataInstant
                    {
                        public class TimeSeriesMemberDataInstantDetails
                        {
                            public double air_pressure_at_sea_level { get; set; }
                            public double air_temperature { get; set; }
                            public double cloud_area_fraction { get; set; }
                            public double relative_humidity { get; set; }
                            public double wind_from_direction { get; set; }
                            public double wind_speed { get; set; }
                        }
                        public TimeSeriesMemberDataInstantDetails details { get; set; } = null!;
                    }
                    public class NextHours
                    {
                        public class NextHoursSummary
                        {
                            public string symbol_code { get; set; } = null!;
                        }
                        public class NextHoursDetails
                        {
                            public double precipitation_amount { get; set; }
                        }
                        public NextHoursSummary summary { get; set; } = null!;
                        public NextHoursDetails? details { get; set; }
                    }
                    public TimeSeriesMemberDataInstant instant { get; set; } = null!;
                    public NextHours? next_12_hours { get; set; }
                    public NextHours? next_1_hours { get; set; }
                    public NextHours? next_6_hours { get; set; }
                }
                public string time { get; set; } = null!;
                public TimeSeriesMemberData data { get; set; } = null!;
            }
            public TimeSeriesMember[] timeseries { get; set; } = null!;
        }
        public Properties properties { get; set; } = null!;
    }
    public class WeatherForecastService
    {
        private readonly HttpClient client = new HttpClient();
        public WeatherForecastService()
        {
            client.DefaultRequestHeaders.Add("User-Agent", "C# program");
        }
        public async Task<List<OneDayWeatherForecast>> getForecastAsync(int altitude, double latitude, double longitude, double baseTemperature, double currentGdd)
        {
            string forecastApiResponseJson = await client.GetStringAsync(" https://api.met.no/weatherapi/locationforecast/2.0/compact?altitude=" + altitude + "&lat=" + latitude + "&lon=" + longitude);
            ForecastApiResponse forecastApiResponse = JsonConvert.DeserializeObject<ForecastApiResponse>(forecastApiResponseJson)!;
            List<OneDayWeatherForecast> forecasts = new List<OneDayWeatherForecast>();
            string lastDate = forecastApiResponse.properties.timeseries[0].time.Split('T')[0];
            int index = 0;
            double temperatureSum = 0;
            double precipitationSum = 0;
            double gdd = currentGdd;
            double tMin = forecastApiResponse.properties.timeseries[0].data.instant.details.air_temperature;
            double tMax = tMin;
            int timesCount = 0;
            foreach (ForecastApiResponse.Properties.TimeSeriesMember timeSeriesMember in forecastApiResponse.properties.timeseries)
            {
                string thisDate = timeSeriesMember.time.Split('T')[0];
                double thisTemperature = timeSeriesMember.data.instant.details.air_temperature;
                if (thisDate != lastDate)
                {
                    double thisGdd = Utils.getGdd(tMin, tMax, baseTemperature);
                    if (thisGdd > 0)
                        gdd += thisGdd;
                    forecasts.Add(new OneDayWeatherForecast()
                    {
                        Date = lastDate,
                        averageTemperature = temperatureSum / timesCount,
                        averagePrecipitation = precipitationSum / timesCount,
                        GDD = gdd
                    });
                    index++;
                    if (index == 8)
                        break;
                    temperatureSum = 0;
                    precipitationSum = 0;
                    timesCount = 0;
                    tMin = thisTemperature;
                    tMax = tMin;
                    lastDate = thisDate;
                }
                temperatureSum += thisTemperature;
                try
                {
                    precipitationSum += timeSeriesMember.data.next_6_hours!.details!.precipitation_amount;
                }
                catch (Exception)
                {
                    // This error can be ignored
                }
                timesCount++;
                if (thisTemperature < tMin)
                    tMin = thisTemperature;
                else if (thisTemperature > tMax)
                    tMax = thisTemperature;
            }
            return forecasts;
        }
    }
}