﻿namespace Sales.Domain.Models
{
    using Sales.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {

        }
        public DbSet<Category> Categories { get; set; }

        //Mapea la clase Products a la base de datos
        public DbSet<Product> Products { get; set; }

        public DbSet<Video> Videos { get; set; }
    }
}
