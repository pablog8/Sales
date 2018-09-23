namespace Sales.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Globalization;

    public interface ILocalize
    {
        //para saber en que idioma está el teléfono
        CultureInfo GetCurrentCultureInfo();

        //cambiar configuración al telefono
        void SetLocale(CultureInfo ci);

    }
}
