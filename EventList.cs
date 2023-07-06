using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Project.Views.OrganizatonPage;

namespace Project.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrganizatonPage : ContentPage
    {
        public string access_token;
        public string refresh_token;
        public string refreshToken;


        public OrganizatonPage()
        {
            InitializeComponent();
            access_token = Preferences.Get("accessTokenKey", "");
            if (access_token != null)
            {
                GetOrganization();
            }
            else if (access_token == null)
            {
                ApiCenter apiCenter = new ApiCenter();
                var accessToken = apiCenter.RefreshService();
                if (accessToken != null)
                {
                    GetOrganization();
                }
                else
                {
                    DisplayAlert("Error", "Token expired", "Close");
                    Preferences.Clear();
                    Shell.Current.GoToAsync("//login");
                }
            }
        }

        private async void OnOrganizationSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedEvent = (Event)e.SelectedItem;
            int selectedId = selectedEvent.Id;

            // ↓ It transmits the selected value when switching to the detail page. ↓
            await Navigation.PushAsync(new OrganizationDetailPage(selectedId));

            // Clear selection
            OrganizationListView.SelectedItem = null;
        }


        private async void GetOrganization()
        {
            ApiCenter apiCenter = new ApiCenter();
            var jObject = await apiCenter.EventList();
            if (jObject != null)
            {
                List<Event> EventList = new List<Event>();
                List<Ticket> myTicket = new List<Ticket>();

                foreach (var item in jObject)
                {
                    JToken eventData = item.Value["event"];

                    foreach (var item2 in eventData)
                    {
                        EventList.Add(new Event()
                        {
                            Id = int.Parse(item2["id"].ToString()),
                            Title = item2["title"].ToString(),
                            EventTime = item2["event_time"].ToString(),
                            Content = item2["content"].ToString(),
                            LocationName = item2["location_name"].ToString(),
                        });

                    }
                    OrganizationListView.ItemsSource = EventList;

                }
            }
            else
            {
                DisplayAlert("Error", "Token expired", "Close");
                Preferences.Clear();
                Shell.Current.GoToAsync("//login");
            }
        }
    }
}