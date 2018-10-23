namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Views;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Xamarin.Forms;


    public class MainViewModel
    {
        #region Properties
        public LoginViewModel Login { get; set; }

        public EditProductViewModel EditProduct { get; set; }

        public ProductsViewModel Products { get; set; }

        public AddProductViewModel AddProduct { get; set; }

        public RegisterViewModel Register { get; set; }

        public MyUserASP UserASP { get; set; }

        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

        public string UserFullName
        {
            get
            {
                //Si el usuario no es nulo y los datos claims son mayores que uno
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 1)
                {
                    //devolvemos claim 0 y claim 1 que es el nombre y apellidos
                    return $"{this.UserASP.Claims[0].ClaimValue} {this.UserASP.Claims[1].ClaimValue}";
                }

                return null;
            }
        }


        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.LoadMenu();
            //this.Products = new ProductsViewModel();
        }


        #endregion

        #region Methods

        private void LoadMenu()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_info",
                PageName = "AboutPage",
                Title = Languages.About,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_phonelink_setup",
                PageName = "SetupPage",
                Title = Languages.Setup,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = Languages.Exit,
            });
        }


        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion

        #region Commands
        //cuando toque en el icono +, lanza el comando AddProductComand, que devuelve la pagina del metodo GoToAddProduct
        public ICommand AddProductCommand
        {
            get
            {
                return new RelayCommand(GoToAddProduct);
            }
        }

        private async void GoToAddProduct()
        {
            //Antes de lanzar la pagina instanciamos la viewmodel que gobierna la pagina
            this.AddProduct = new AddProductViewModel();

            await App.Navigator.PushAsync(new AddProductPage());
        }
        #endregion
    }
}
