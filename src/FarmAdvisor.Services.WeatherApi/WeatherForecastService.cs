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
                            public double air_pressure_at_sea_level;
                            public double air_temperature;
                            public double cloud_area_fraction;
                            public double relative_humidity;
                            public double wind_from_direction;
                            public double wind_speed;
                        }
                        public TimeSeriesMemberDataInstantDetails details = null!;
                    }
                    public class NextHours
                    {
                        public class NextHoursSummary
                        {
                            public string symbol_code = null!;
                        }
                        public class NextHoursDetails
                        {
                            public double precipitation_amount;
                        }
                        public NextHoursSummary summary = null!;
                        public NextHoursDetails? details;
                    }
                    public TimeSeriesMemberDataInstant instant = null!;
                    public NextHours? next_12_hours;
                    public NextHours? next_1_hours;
                    public NextHours? next_6_hours;
                }
                public string time = null!;
                public TimeSeriesMemberData data = null!;
            }
            public TimeSeriesMember[] timeseries = null!;
        }
        public Properties properties = null!;
    }
    public class WeatherForecastService
    {
        private readonly HttpClient client = new HttpClient();
        public WeatherForecastService()
        {

            client.DefaultRequestHeaders.Add("User-Agent", "C# program");
        }
        public async Task<List<OneDayWeatherForecast>> getForecastAsync(int altitude, double latitude, double longitude, double baseTemperature)
        {
            string forecastApiResponseJson = await client.GetStringAsync(" https://api.met.no/weatherapi/locationforecast/2.0/compact?altitude=" + altitude + "&lat=" + latitude + "&lon=" + longitude);
            ForecastApiResponse forecastApiResponse = JsonConvert.DeserializeObject<ForecastApiResponse>(forecastApiResponseJson)!;
            List<OneDayWeatherForecast> forecasts = new List<OneDayWeatherForecast>();
            string lastDate = forecastApiResponse.properties.timeseries[0].time.Split('T')[0];
            int index = 0;
            double temperatureSum = 0;
            double precipitationSum = 0;
            double tMin = forecastApiResponse.properties.timeseries[0].data.instant.details.air_temperature;
            double tMax = tMin;
            int timesCount = 0;
            foreach (ForecastApiResponse.Properties.TimeSeriesMember timeSeriesMember in forecastApiResponse.properties.timeseries)
            {
                string thisDate = timeSeriesMember.time.Split('T')[0];
                double thisTemperature = timeSeriesMember.data.instant.details.air_temperature;
                if (thisDate != lastDate)
                {
                    forecasts.Add(new OneDayWeatherForecast()
                    {
                        Date = lastDate,
                        averageTemperature = temperatureSum / timesCount,
                        averagePrecipitation = precipitationSum / timesCount,
                        GDD = Utils.getGdd(tMin, tMax, baseTemperature)
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
                catch (Exception) { }
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