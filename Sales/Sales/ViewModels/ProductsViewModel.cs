namespace Sales.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Windows.Input;
    using Sales.Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Services;
    using Xamarin.Forms;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductsViewModel : BaseViewModel
    {
        #region Attributes
        private string filter;

        private APIService apiService;

        private DataService dataService;

        private bool isRefreshing;

        private ObservableCollection<ProductItemViewModel> products;
        #endregion

        #region Properties
        public string Filter {
            get
            {
                return this.filter;
            }
            set
            {
                this.filter = value;
                this.RefreshList();
            }
        }

        public List<Product> MyProducts { get; set; }
    

        public ObservableCollection<ProductItemViewModel> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructors
        public ProductsViewModel()
        {
            //primera vez que llamamos a ProductsViewModel para guardarla 
            instance = this;

            this.apiService = new APIService();
            this.dataService = new DataService();
            this.LoadProducts();
        }
        #endregion

        //para llamar a una clase existente sin necesitad de volver a instanciarla =>SIGLETON
        #region Singleton
        private static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductsViewModel();
            }
            return instance;
        }
        #endregion

        #region Methods
        //va a la API y almacena una lista en MyProducts y ejecuta RefreshList
        private async void LoadProducts()
        {
            //carga los productos
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            //si la conexion a internet no ha sido exitosa
            if (connection.IsSuccess)
            {
                //comprobamos si cargamos los datos del servidor cuando tenemos conexion a internet, si no la cargamos de la base de datos local
                var answer = await this.LoadProductsFromAPI();
                if (answer)
                {
                    this.SaveProductsToDB();
                }
            }

            else
            {
                await this.LoadProductsFromDB();
                
            }

                if(this.MyProducts == null || this.MyProducts.Count == 0)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.NoProductsMessage, Languages.Accept);
                    return;
                }

            

            this.RefreshList();

           
            this.IsRefreshing = false;
        }

        private async Task LoadProductsFromDB()
        {
            this.MyProducts = await this.dataService.GetAllProducts();
        }

        private async Task SaveProductsToDB()
        {
            await this.dataService.DeleteAllProducts();
            this.dataService.Insert(this.MyProducts);
        }

        private async Task<bool> LoadProductsFromAPI()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();

            var prefix = Application.Current.Resources["UrlPrefix"].ToString();

            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.GetList<Product>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                /*
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                */
                return false;
            }
            this.MyProducts = (List<Product>)response.Result;

            return true;
        }

        public void RefreshList()
        {
            //si no hay filtro en la lupa
            if (string.IsNullOrEmpty(this.Filter))
            {
                var myListProductItemViewModel = this.MyProducts.Select(p => new ProductItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    PublishOn = p.PublishOn,
                    ProductId = p.ProductId,
                    Remarks = p.Remarks,

                });

                this.Products = new ObservableCollection<ProductItemViewModel>(
                    myListProductItemViewModel.OrderBy(p => p.Description));
            }
            //si hay texto en la lupa
            else
            {
                var myListProductItemViewModel = this.MyProducts.Select(p => new ProductItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    PublishOn = p.PublishOn,
                    ProductId = p.ProductId,
                    Remarks = p.Remarks,

                }).Where(p => p.Description.ToLower().Contains(this.Filter.ToLower())).ToList();

                this.Products = new ObservableCollection<ProductItemViewModel>(
                    myListProductItemViewModel.OrderBy(p => p.Description));

            }
            //Convierto la lista de los Products a ProductItemViewModel

            
        }
        #endregion

        #region Commands

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        }
        //carga los productos
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(RefreshList);
            }
        }
        #endregion

    }
}