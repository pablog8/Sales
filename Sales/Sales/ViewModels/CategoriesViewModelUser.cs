﻿namespace Sales.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Sales.Lesiones;
    using Services;
    using Xamarin.Forms;

    public class CategoriesViewModelUser : BaseViewModel
    {
        #region Attributes
        private string filter;

        private APIService apiService;

        private bool isRefreshing;

        private ObservableCollection<CategoryItemViewModelUser> categories;
        #endregion

        #region Properties
        public string Filter
        {
            get { return this.filter; }
            set
            {
                this.filter = value;
                this.RefreshList();
            }
        }

        public List<Category> MyCategories { get; set; }

        public ObservableCollection<CategoryItemViewModelUser> Categories
        {
            get { return this.categories; }
            set { this.SetValue(ref this.categories, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructors
        Deportista deportistaa;
        public CategoriesViewModelUser(Deportista deportista)
        {
            this.apiService = new APIService();
            this.deportistaa = deportista;
            this.LoadCategories();
        }
        #endregion

        #region Methods
        private async void LoadCategories()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlCategoriesController"].ToString();
            var response = await this.apiService.GetList<Category>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            this.MyCategories = (List<Category>)response.Result;
            this.RefreshList();
            this.IsRefreshing = false;
        }

        private void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                var myListCategoriesItemViewModel = this.MyCategories.Select(c => new CategoryItemViewModelUser(this.deportistaa)
                {
                    CategoryId = c.CategoryId,
                    Description = c.Description,
                    ImagePath = c.ImagePath,
                });
                //Arma la observablecollection y comprueba si hay filtro
                this.Categories = new ObservableCollection<CategoryItemViewModelUser>(
                    myListCategoriesItemViewModel.OrderBy(c => c.Description));
            }
            else
            {
                var myListCategoriesItemViewModel = this.MyCategories.Select(c => new CategoryItemViewModelUser (this.deportistaa)
                {
                    CategoryId = c.CategoryId,
                    Description = c.Description,
                    ImagePath = c.ImagePath,
                }).Where(c => c.Description.ToLower().Contains(this.Filter.ToLower())).ToList();

                this.Categories = new ObservableCollection<CategoryItemViewModelUser>(
                    myListCategoriesItemViewModel.OrderBy(c => c.Description));
            }
        }
        #endregion

        #region Commands
        //refresca la lista
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(RefreshList);
            }
        }

        //carga la lista de categorias
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadCategories);
            }
        }
        #endregion
    }

}
