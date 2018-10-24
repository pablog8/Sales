using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Sales
{
    using Newtonsoft.Json;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using Sales.ViewModels;
    using System.Threading.Tasks;
    using Views;
	public partial class App : Application
	{
        public static NavigationPage Navigator { get; internal set; }

        public App ()
		{
			InitializeComponent();

            //preguntamos si ya esta logeado o no
            var mainViewModel = MainViewModel.GetInstance();

            if (Settings.IsRemembered)
            {

                if (!string.IsNullOrEmpty(Settings.UserASP))
                {
                    mainViewModel.UserASP = JsonConvert.DeserializeObject<MyUserASP>(Settings.UserASP);
                }

                mainViewModel.Products = new ProductsViewModel();
                this.MainPage = new MasterPage();
            }
            else
            {
                mainViewModel.Login = new LoginViewModel();
                this.MainPage = new NavigationPage(new LoginPage());
            }


            //MainPage = new NavigationPage(new ProductsPage());
        }
        //si se hace algun login y se sale
        public static Action HideLoginView
        {
            get
            {
                return new Action(() => Current.MainPage = new NavigationPage(new LoginPage()));
            }
        }

        //cuando se hace el login
        public static async Task NavigateToProfile(TokenResponse token)
        {
            if (token == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            //si se obtiene el token
            Settings.IsRemembered = true;
            Settings.AccessToken = token.AccessToken;
            Settings.TokenType = token.TokenType;

            var apiService = new APIService();
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlUsersController"].ToString();
            var response = await apiService.GetUser(url, prefix, $"{controller}/GetUser", token.UserName, token.TokenType, token.AccessToken);
            if (response.IsSuccess)
            {
                var userASP = (MyUserASP)response.Result;
                MainViewModel.GetInstance().UserASP = userASP;
                Settings.UserASP = JsonConvert.SerializeObject(userASP);
            }

            MainViewModel.GetInstance().Products = new ProductsViewModel();
            Application.Current.MainPage = new MasterPage();
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
