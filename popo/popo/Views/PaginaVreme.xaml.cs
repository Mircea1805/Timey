using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherBackground.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeatherBackground.Models;
using Xamarin.Essentials;

namespace WeatherBackground.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaVreme : ContentPage
    {
        public PaginaVreme()
        {
            InitializeComponent();
            GetCoordinates();
        }

        private string Location { get; set; } = "Vladivostok";
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        private async void GetCoordinates()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if(location!=null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    Location = await GetCity(location);
                    GetWeatherInfo();
                }

            }
            catch(Exception ex)
            { Console.WriteLine(ex.StackTrace);
            
            }
                
        }

        private async Task<string> GetCity(Location location)
        {
            var places = await Geocoding.GetPlacemarksAsync(location);
            var currentPlace = places?.FirstOrDefault();

            if(currentPlace!=null)
            {

                return $"{currentPlace.Locality},{currentPlace.CountryName}";

            }
            return null;
        }

        private async void GetWeatherInfo()
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?q={Location}&appid=f7562a2d373e9654de8197ee602b668e&units=metric";
            var result = await ApiCaller.Get(url);

            if(result.Successful)
            { 
                try
                {   var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);
                    descriptionTxt.Text = weatherInfo.weather[0].description.ToUpper();
                    iconImg.Source = $"w{weatherInfo.weather[0].icon}";
                    cityTxt.Text = weatherInfo.name.ToUpper();
                    temperatureTxt.Text = weatherInfo.main.temp.ToString("0");
                    humidityTxt.Text = $"{weatherInfo.main.humidity}%";
                    pressureTxt.Text = $"{weatherInfo.main.pressure} hpa";
                    windTxt.Text = $"{weatherInfo.wind.speed} m/s";
                    cloudinessTxt.Text = $"{weatherInfo.clouds.all}%";

                    var dt = new DateTime().ToUniversalTime().AddSeconds(weatherInfo.dt);
                    dateTxt.Text = dt.ToString("dddd, MMM dd").ToUpper();
                    if(weatherInfo.weather[0].main=="Rain")
                    bgImg.Source = "ploaieiar.gif";
                    if (weatherInfo.weather[0].main == "Clear")
                        bgImg.Source = "ploaieiar.gif";
                    if (weatherInfo.weather[0].main == "Clouds")
                        bgImg.Source = "nor.gif";
                    if (weatherInfo.weather[0].main == "Snow")
                        bgImg.Source = "snow_gif_11.gif";
                    if (weatherInfo.weather[0].main == "Drizzle")
                        bgImg.Source = "drizzle.gif";
                    if (weatherInfo.weather[0].main == "Thunderstorm")
                        bgImg.Source = "storm_2.gif";
                }
                catch (Exception ex)
                { await DisplayAlert("Weather Info", ex.Message, "OK"); }
            
            }
            else 
            {
                await DisplayAlert("Weather Info", "No weather information found", "OK");
            }
        }

    }
}