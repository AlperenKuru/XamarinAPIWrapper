using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Project.Views.OrganizatonPage;
using static Project.Views.ProfilePage;

namespace Project.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public string access_token;
        public string refresh_token;
        public string jwtToken;
        public string userId;


        public ProfilePage()
        {
            InitializeComponent();
            access_token = Preferences.Get("accessTokenKey", "");
            if (access_token != null)
            {
                GetProfile();
            }
            else if (access_token == null)
            {
                ApiCenter apiCenter = new ApiCenter();
                var accessToken = apiCenter.RefreshService();
                if (accessToken != null)
                {
                    GetProfile();
                }
                else
                {
                    DisplayAlert("Error", "Token expired", "Close");
                    Preferences.Clear();
                    //buradan crash verebilir await yok
                    Shell.Current.GoToAsync("//login");
                }
            }
        }

        private void jwtParser()
        {
            jwtToken = access_token;
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

                // Token'dan gelen bilgileri almak için kullanılabilir
                userId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

                Preferences.Set("userJWTId", userId);
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Token çözme hatası: {ex.Message}");
                DisplayAlert("Error", "Failed to parse token.!", "Close");
            }
        }

        

        private async void GetProfile()
        {

            jwtParser();
            ApiCenter apiCenter = new ApiCenter();
            var jObject = await apiCenter.RiderById(userId);
            if (jObject != null) 
            {
                List<UserData> UserDataList = new List<UserData>();
                List<User> UserList = new List<User>();

                foreach (var item in jObject)
                {
                    JToken riderData = item.Value["rider_profile"];

                    foreach (var item2 in riderData)
                    {
                        UserDataList.Add(new UserData()
                        {
                            id = item2["id"].ToString(),
                            riderId = item2["rider_id"].ToString(),
                            charter = item2["charter"].ToString(),
                            rank = item2["rank"].ToString(),
                            nickname = item2["nickname"].ToString(),
                            bloodType = item2["blood_type"].ToString()
                        });

                        lblUser.Text = $"{item2["user"]["first_name"].ToString()} {item2["user"]["last_name"].ToString()}";
                        lblGrade.Text = $"{item2["rank"].ToString()}";
                    }
                }
            }
            else 
            {
                DisplayAlert("Error", "Token expired", "Close");
                Preferences.Clear();
                await Shell.Current.GoToAsync("//login");
            }

            
        }
    }
}