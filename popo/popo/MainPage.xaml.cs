using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using WeatherBackground.Helper;
using WeatherBackground.Models;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;

namespace Timey
{

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            GetCoordinates();
        }

        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            TodoItem todoItemBeingRemoved = sender as TodoItem;
            if (e.Value == true)
            {
                App.database.DeleteItemAsync(todoItemBeingRemoved);
            }
            else
            {
                App.database.SaveItemAsync(todoItemBeingRemoved);
                todoItemBeingRemoved.Complete = false;
            }
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

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    Location = await GetCity(location);
                    GetWeatherInfo();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

            }

        }

        private async Task<string> GetCity(Location location)
        {
            var places = await Geocoding.GetPlacemarksAsync(location);
            var currentPlace = places?.FirstOrDefault();

            if (currentPlace != null)
            {

                return $"{currentPlace.Locality},{currentPlace.CountryName}";

            }
            return null;
        }

        private async void GetWeatherInfo()
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?q={Location}&appid=f7562a2d373e9654de8197ee602b668e&units=metric";
            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                try
                {
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);
                    descriptionTxt.Text = weatherInfo.weather[0].description.ToUpper();
                    iconImg.Source = $"w{weatherInfo.weather[0].icon}";
                    cityTxt.Text = weatherInfo.name.ToUpper();
                    temperatureTxt.Text = weatherInfo.main.temp.ToString("0");

                    var dt = new DateTime().ToUniversalTime().AddSeconds(weatherInfo.dt);
                    dateTxt.Text = dt.ToString("dddd, MMM dd").ToUpper();
                    if (weatherInfo.weather[0].main == "Rain")
                        bgImg.Source = "ploaieiar.gif";
                    if (weatherInfo.weather[0].main == "Clear")
                        bgImg.Source = "cumustata.gif";
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


#pragma warning disable IDE1006 // Naming Styles
        private void btnSkillPopup_Clicked (object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            popupAddSkillsView.IsVisible = true;
            DateField.MinimumDate = DateTime.Now;
        }

        void OnImageNameTapped_ClosePopup(object sender, EventArgs e)
        {
            popupAddSkillsView.IsVisible = false;
        }
        public void HandleEnterPress(object sender, EventArgs args)
        {
            InputField.Text = "";
            DateField.MinimumDate= DateTime.Now;
            popupAddSkillsView.IsVisible = false;

        }
        
    }
}
