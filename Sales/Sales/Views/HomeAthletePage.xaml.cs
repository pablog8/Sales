using Sales.Lesiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sales.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeAthletePage : ContentPage
    {
        public HomeAthletePage()
        {
            InitializeComponent();

            //listaListView.RowHeight = 70;

            this.Padding = Device.OnPlatform(
            new Thickness(10, 20, 10, 10),
            new Thickness(10, 10, 10, 10),
            new Thickness(10, 10, 10, 10));



            //  listaListView.ItemTemplate = new DataTemplate(typeof(DeportistaCell));
            listaListView.RowHeight = 70;
            //mostrar la lista con los datos previamente ingresados



            nuevo.Clicked += Nuevo_Clicked;
            listaListView.ItemSelected += ListaListView_ItemSelected;
        }

        private async void Nuevo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAthlete());
        }

        //actualizala lista en la pantalla
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (var datos = new DataAccess())
            {
                listaListView.ItemsSource = datos.GetDeportistas();
            }
        }

        private async void ListaListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new EditAthlete((Deportista)e.SelectedItem));
        }


    }
}