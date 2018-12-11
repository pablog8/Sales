using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.ViewModels
{
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Sales.Views;

    //hereda de category para mantener el modelo category puro sin métodos
    public class CategoryItemViewModel : Category
    {
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
            /*
            if (MainViewModel.GetInstance().UserASP.Email == "prueba3@usal.es")
            {
                MainViewModel.GetInstance().Products = new ProductsViewModel(this);// (this);
                await App.Navigator.PushAsync(new ProductsPageUser());
            }*/
            
                MainViewModel.GetInstance().Products = new ProductsViewModel(this);// (this);
                await App.Navigator.PushAsync(new ProductsPage());
            
            
        }
        #endregion
    }

}
