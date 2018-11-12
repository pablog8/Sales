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
    public partial class AddExercise : ContentPage
    {
        Deportista deportistaa;
        //public List<string> listaemail = new List<string>();
        public AddExercise(Deportista deportista)
        {
            InitializeComponent();
            deportistaa = deportista;
            agregarlesion.Clicked += Agregarlesion_Clicked;
            //listaemail = new List<string>();
        }

        private async void Agregarlesion_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nombresejercicio.Text))
            {
                await DisplayAlert("Error", "Debe ingresar nombres", "Aceptar");
                nombresejercicio.Focus();
                return;
            }
            if (string.IsNullOrEmpty(descripcionEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar una descripción del ejercicio", "Aceptar");
                descripcionEntry.Focus();
                return;




            }
            //creamos el deportista
            var ejercicio = new TablaEjercicios
            {
                Nombreejercicio = nombresejercicio.Text,
                Descripcion = descripcionEntry.Text,
                clavedeportista = deportistaa.IDDeportista,

                // Salario = decimal.Parse(salarioEntry.Text),

            };

            //insertamos el deportista en la base de datos
            using (var datos = new DataAccess())
            {
                datos.InsertEjercicio(ejercicio);
                //listaListView.ItemsSource = datos.GetDeportistas();
            }
            // listaemail.Add("Nombre: " + ejercicio.Nombreejercicio+"Series: "+ejercicio.Series+ Environment.NewLine);

            nombresejercicio.Text = string.Empty;
            descripcionEntry.Text = string.Empty;
            ////emailEntry.Text = string.Empty;
            // salarioEntry.Text = string.Empty;
            // fechaContratoDatePicker.Date = DateTime.Now;
            //// activoSwitch.IsToggled = true;
            await DisplayAlert("Confirmación", "ejercicio agregado", "Aceptar");
            //  await Navigation.PushAsync(new Trabajo.HomePage());
        }






    }
}