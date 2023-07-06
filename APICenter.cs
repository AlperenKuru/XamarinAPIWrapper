using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static Project.Views.OrganizatonPage;
using System.Net.Http.Headers;
using Project.Models;

namespace Project.Views
{
    internal class ApiCenter
    {
        public string access_token;
        public string refresh_token;

        // Sample Login API Call
        public async Task<string> Login(string email, string password)
        {
            // ↓ You Query ↓
            var query = @"
        mutation ($email: String!, $password: String!) {
            auth_login(email: $email, password: $password) {
                access_token
                refresh_token
            }
        }";
            // ↓ You variables ↓
            var variables = new
            {
                email,
                password
            };

            // ↓ You in include in the query and variable ↓
            var payload = new
            {
                query,
                variables
            };

            using (var client = new HttpClient())
            {
                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync("Your_Request_URL", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseContent);
                var access_token = (string)jObject["data"]["auth_login"]["access_token"];
                var refresh_token = (string)jObject["data"]["auth_login"]["refresh_token"];
                Preferences.Set("accessTokenKey", access_token);
                Preferences.Set("refreshTokenKey", refresh_token);

                return access_token;
            }
        }

        //Sample Refresh Token API Call
        public async Task<string> RefreshService()
        {
            refresh_token = Preferences.Get("refreshTokenKey", "");
            // ↓ You Query ↓
            var query = @"
            mutation ($refresh_token: String!) {
	auth_refresh(refresh_token: $refresh_token, mode: json) {
		access_token
		refresh_token
	}
}";
            // ↓ You variables ↓
            var variables = new
            {
                refresh_token
            };
            // ↓ You in include in the query and variable ↓
            var payload = new
            {
                query,
                variables
            };

            using (var client = new HttpClient())
            {
                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync("Your_Request_URL", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseContent);
                access_token = (string)jObject["data"]["auth_refresh"]["access_token"];
                refresh_token = (string)jObject["data"]["auth_refresh"]["refresh_token"];
                Preferences.Set("accessTokenKey", access_token);
                Preferences.Set("refreshTokenKey", refresh_token);
            }
            return access_token;
        }


        //Sample EventList API Call
        public async Task<JObject> EventList()
        {

            access_token = Preferences.Get("accessTokenKey", "");
            // ↓ You Query ↓
            var query = @"query {
                        event {
                            id, 
                            title, 
                            event_time, 
                            privacy,
                            is_weekly,
                            location_name,
                            cover_image { id, filename_disk },
                            tickets { name, price, currency },
                            attendees { 
                                user { id, first_name, last_name }, 
                                status  
                            }
                        }
                    }";
            // ↓ You variables ↓
            var variables = new
            {
                //şuanda tüm listeyi çekiyor charter bazlı çekmek için bunanın içerisinde "id" değeri göndermen gerekecek. id değerini kullanıcının bağlı olduğu user bilgisinden çekebilirsin
            };

            // ↓ You in include in the query and variable ↓
            var payload = new
            {
                query
                // variables
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync("Your_Request_URL", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseContent);

                return jObject;
            }
        }
    }
}