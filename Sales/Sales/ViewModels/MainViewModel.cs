﻿namespace Sales.ViewModels
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

        public VideosViewModel Videos { get; set; }

        public CategoriesViewModel Categories { get; set; }

        public EditProductViewModel EditProduct { get; set; }

        public ProductsViewModel Products { get; set; }

        public AddProductViewModel AddProduct { get; set; }

        public AddAthleteViewModel AddAthlete { get; set; }

        public RegisterViewModel Register { get; set; }

        public ContactViewModel Contact { get; set; }

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

        public string UserImageFullPath
        {
            get
            {
                foreach (var claim in this.UserASP.Claims)
                {
                    if (claim.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri")
                    {
                        if (claim.ClaimValue.StartsWith("~"))
                        {
                            return $"http://salesapiservices2018.azurewebsites.net{claim.ClaimValue.Substring(1)}";
                        }

                        return claim.ClaimValue;
                    }
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

        //opciones del menú
        #region Methods

        private void LoadMenu()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>();

            /*
            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "message",
                PageName = "Presentation",
                Title = Languages.Presentation,
            
            */
            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "user",
                PageName = "Paciente",
                Title = "Gestión de lesiones",
            });
            /*
            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_info",
                PageName = "AboutPage",
                Title = Languages.About,
            });
            */
            
            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "contacts",
                PageName = "Exercises",
                Title = "Ejercicios",
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "videos",
                PageName = "Videos",
                Title = "Videos",
            });


            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "information",
                PageName = "Contact",
                Title = "Contacto",
            });
            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "iconfinanciacion",
                PageName = "Financiacion",
                Title = "Financiación",
            });
            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "exit",
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
