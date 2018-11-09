using Plugin.Media.Abstractions;
using Sales.Common.Models;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class MyProfileViewModel : BaseViewModel
    {
        #region Attributes
        //para fotos
        private MediaFile file;
        private ImageSource imageSource;

        private APIService apiService;

        private bool isRunning;

        private bool isEnabled;

        #endregion

        #region Properties
        

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

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }

        private MyUserASP UserASP
        {
            get;
            set;
        }
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

        #region Constructor
        //el usuario esta almacenado en la mainviewmodel

        public MyProfileViewModel()
        {
            //el usuario esta almacenado en la mainviewmodel
            this.UserASP = MainViewModel.GetInstance().UserASP;
            this.apiService = new APIService();
            //inicializamos el boton
            
            this.IsEnabled = true;
            
        }
        #endregion
    }
}
