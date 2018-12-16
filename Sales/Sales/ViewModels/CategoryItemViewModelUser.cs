using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.ViewModels
{
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Sales.Lesiones;
    using Sales.Views;

    //hereda de category para mantener el modelo category puro sin métodos
    public class CategoryItemViewModelUser : Category
    {
        Deportista deportistaa;
        public CategoryItemViewModelUser(Deportista deportista)
        {
            //this.apiService = new APIService();
            this.deportistaa = deportista;
            //this.LoadCategories();
        }
       
        
        #region Commands
        public ICommand GotoCategoryCommand
        {
            get
            {
                return new RelayCommand(GotoCategory);
            }
        }

        private async void GotoCategory()
        {
            
            if (MainViewModel.GetInstance().UserASP.Email == "prueba3@usal.es")
            {
                MainViewModel.GetInstance().Productss = new ProductsViewModelUser(this, this.deportistaa);// (this);
                await App.Navigator.PushAsync(new ProductsPage());
            }
            else
            {
                MainViewModel.GetInstance().Products = new ProductsViewModel(this);// (this);
                await App.Navigator.PushAsync(new ProductsPageUser());
            }




        }
        #endregion
    }

}
