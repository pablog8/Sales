namespace Sales.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Plugin.Geolocator;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using Xamarin.Forms;
    using Xamarin.Forms.Maps;

    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
        }

        //Cuando la pagina carga
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //localizar la ubicación en el mapa
            this.Locator();
        }

        //utiliza el plugin xamarin.plugin.geolocator
        private async void Locator()
        {
            var locator = CrossGeolocator.Current;
            //precisión de 50m
            locator.DesiredAccuracy = 50;


            //obtiene donde nos localizamos
            var location = await locator.GetPositionAsync();
            //con la localizacion obtiene nuestra posicion exacta
            var position = new Position(location.Latitude, location.Longitude);
            //mover el mapa hasta donde estamos, y situa la camara 1km encima
            this.MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));

            try
            {
                this.MyMap.IsShowingUser = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            var pins = await this.GetPins();
            this.ShowPins(pins);

        }
        private void ShowPins(List<Pin> pins)
        {
            foreach (var pin in pins)
            {
                this.MyMap.Pins.Add(pin);
            }
        }

        private async Task<List<Pin>> GetPins()
        {
            var pins = new List<Pin>();
            //Trae la lista de productos en los que la longitud y latitud no sean nulos
            var apiService = new APIService();
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await apiService.GetList<Product>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            var products = (List<Product>)response.Result;
            foreach (var product in products.Where(p => p.Latitude != 0 && p.Longitude != 0).ToList())
            {
                var position = new Position(product.Latitude, product.Longitude);
                pins.Add(new Pin
                {
                    Address = product.Remarks,
                    Label = product.Description,
                    Position = position,
                    Type = PinType.Place,
                });
            }

            return pins;
        }


        //para mover el slider
        private void Handle_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            var zoomLevel = double.Parse(e.NewValue.ToString()) * 18.0;
            var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
            this.MyMap.MoveToRegion(new MapSpan(this.MyMap.VisibleRegion.Center, latlongdegrees, latlongdegrees));
        }
    }
}