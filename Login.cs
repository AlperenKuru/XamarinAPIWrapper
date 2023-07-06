using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        string email;
        string password;

        public LoginPage()
        {
            InitializeComponent();
            Preferences.Clear();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            email = UsrEmail.Text;
            password = UsrPassword.Text;

            ApiCenter apiCenter = new ApiCenter();
            var accessToken = await apiCenter.Login(email, password);

            if (!string.IsNullOrEmpty(accessToken))
            {
                await Shell.Current.GoToAsync("//main");
            }
            else
            {
                DisplayAlert("Error", "User information is incorrect", "Close");
            }
        }
    }
}