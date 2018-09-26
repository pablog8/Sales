namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Services;
    using Sales.Common.Models;

    public class AddProductViewModel : BaseViewModel
    {
        #region Attributes

        private APIService apiService;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public string Description { get; set; }

        public string Price { get; set; }

        public string Remarks { get; set; }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        #endregion

        #region Constructors
        public AddProductViewModel()
        {
            this.apiService = new APIService();
            this.isEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error, 
                    Languages.DescriptionError,
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }
            var price = decimal.Parse(this.Price);
            if(price < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }
            this.isRunning = true;
            this.isEnabled = false;

            //chekea la conexion
            var connection = await this.apiService.CheckConnection();
            //si la conexion a internet no ha sido exitosa
            if (!connection.IsSuccess)
            {
                this.isRunning = false;
                this.isEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;

            }

            //metemos lo que mandemos al post como un producto
            var product = new Product
            {
                Description = this.Description,
                Price = price,
                Remarks = this.Remarks
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();

            var prefix = Application.Current.Resources["UrlPrefix"].ToString();

            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            //invocamos el metodo post del apiservice
            var response = await this.apiService.Post(url, prefix, controller, product);

            //preguntamos si lo grabó de manera exitosa
            if (!response.IsSuccess)
            {
                this.isRunning = false;
                this.isEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            //si lo hizo de manera exitosa hacemos el back
            this.isRunning = false;
            this.isEnabled = true;
            //Desapilamos
            await Application.Current.MainPage.Navigation.PopAsync();

        }
        #endregion
    }
}
