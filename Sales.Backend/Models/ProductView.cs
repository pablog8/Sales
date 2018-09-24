using Sales.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales.Backend.Models
{
    public class ProductView : Product
    {
        //heredamos del producto para agregar un nuevo atributo al modelo
        public HttpPostedFileBase ImageFile { get; set; }
    }
}