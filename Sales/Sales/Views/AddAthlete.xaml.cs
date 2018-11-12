﻿using Sales.Interfaces;
using Sales.Lesiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sales.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAthlete : ContentPage
    {
        public AddAthlete()
        {
            InitializeComponent();

            agregarButton.Clicked += AgregarButton_Clicked;

        }


        private async void AgregarButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nombresEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar nombres", "Aceptar");
                nombresEntry.Focus();
                return;
            }
            if (string.IsNullOrEmpty(apellidosEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar apellidos", "Aceptar");
                apellidosEntry.Focus();
                return;
            }

            //COMPROBACIÓN DEL EMAIL
            if (string.IsNullOrEmpty(emailEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un email", "Aceptar");
                emailEntry.Focus();
                return;
            }
            var email = emailEntry.Text;
            var nombre = nombresEntry.Text;
            var emailPattern = "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";

            if (!String.IsNullOrWhiteSpace(email) && !(Regex.IsMatch(email, emailPattern)))
            {
                await DisplayAlert("Error", "Debe ingresar un email válido", "Aceptar");
                emailEntry.Focus();
                return;
            }
            else
            {


                var fromAddress = new MailAddress("pablogf21096@gmail.com", "KAPTA");
                var toAddress = new MailAddress(email, nombre);
                const string fromPassword = "Pasesionthebest";
                const string subject = "EQUIPO KAPTA";
                const string body = "¡Gracias por usar nuestra aplicación!";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = "Hola " + nombresEntry.Text + " ¡Te acabas de dar de alta como deportista en KAPTA! Para más información visita nuestra App",
                })

                {
                    smtp.Send(message);
                }

                /*
                if (string.IsNullOrEmpty(salarioEntry.Text))
                {
                    await DisplayAlert("Error", "Debe ingresar salario", "Aceptar");
                    salarioEntry.Focus();
                    return;
                }
                */



            }
            //creamos el deportista
            var deportista = new Deportista
            {
                Nombres = nombresEntry.Text,
                Apellidos = apellidosEntry.Text,
                Email = emailEntry.Text,
                FechaNacimiento = fechaContratoDatePicker.Date,

                // Salario = decimal.Parse(salarioEntry.Text),
                Activo = activoSwitch.IsToggled
            };

            //insertamos el deportista en la base de datos
            using (var datos = new DataAccess())
            {
                datos.InsertDeportista(deportista);
                //listaListView.ItemsSource = datos.GetDeportistas();
            }
            nombresEntry.Text = string.Empty;
            apellidosEntry.Text = string.Empty;
            emailEntry.Text = string.Empty;
            // salarioEntry.Text = string.Empty;
            fechaContratoDatePicker.Date = DateTime.Now;
            activoSwitch.IsToggled = true;
            DependencyService.Get<IMessage>().LongAlert("Deportista agregado");
            //await DisplayAlert("Confirmación", "Deportista agregado", "Aceptar");
            //  await Navigation.PushAsync(new Trabajo.HomePage());


        }
    }
}