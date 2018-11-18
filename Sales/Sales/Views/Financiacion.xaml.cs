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
    public partial class Financiacion : ContentPage
    {
        public Financiacion()
        {
            InitializeComponent();

            var browser = new WebView();
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body><h1>Xamarin.Forms</h1><p>Welcome to WebView.</p><img src="XamarinLogo.png" /></body></html>";
            browser.Source = htmlSource;
            /*
            browser.Source = htmlSource;
            HtmlWebViewSource personHtmlSource = new HtmlWebViewSource();
            personHtmlSource.SetBinding(HtmlWebViewSource.HtmlProperty, "HTMLDesc");
            personHtmlSource.Html = @"<html ><body style='margin-top:20;margin-left:20;margin-right:20;' ><center></center><b> Kapta </b>es una aplicación ideada y programada por Pablo García Fernández y ha sido financiada por:<img src="XamarinLogo.png";></img></body></html>";
            var browser = new WebView();
            browser.Source = personHtmlSource;
            Content = browser;*/
        }

        public object XamarinLogo = "patrocinadores.png";

    }
}
