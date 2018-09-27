﻿namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Views;
    using Services;
    using System;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductItemViewModel : Product
    {

        #region Attributes
        private APIService apiService;
        #endregion

        #region Constructors
        public ProductItemViewModel()
        {
            this.apiService = new APIService();
        }

        #endregion


        #region Commands
        public ICommand EditProductCommand
        {
            get
            {
                return new RelayCommand(EditProduct);
            }
        }

        private async void EditProduct()
        {

            //Creamos una instancia y ligarlo a la viewmodel
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);

            //tiene que apilar otra pagina
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
            
        }

        public ICommand DeleteProductCommand
        {
            get
            {
                return new RelayCommand(DeleteProduct);
            }
        }

        private async void DeleteProduct()
        {
            var answer =  await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm, 
                Languages.DeleteConfirmation, 
                Languages.Yes, 
                Languages.No);
            if (!answer)
            {
                return;
            }
            //eliminamos el producto
            //comprobamos si hay conexión
            var connection = await this.apiService.CheckConnection();
            //si la conexion a internet no ha sido exitosa
            if (!connection.IsSuccess)
            {
               
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;

            }

            //vamos a la api
            var url = Application.Current.Resources["UrlAPI"].ToString();

            var prefix = Application.Current.Resources["UrlPrefix"].ToString();

            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.Delete(url, prefix, controller, this.ProductId);

            if (!response.IsSuccess)
            {
               
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);

                return;
            }

            //hay que actualizar la lista, llamamos a  SINGLETON
            var productsViewModel = ProductsViewModel.GetInstance();

            //buscamos el producto en la lista y lo eliminamos
            var deletedProduct = productsViewModel.Products.Where(p => p.ProductId == this.ProductId).FirstOrDefault();

            //si encontramos el producto
            if (deletedProduct != null) {
                productsViewModel.Products.Remove(deletedProduct);
            }

        }

        #endregion
    }
}