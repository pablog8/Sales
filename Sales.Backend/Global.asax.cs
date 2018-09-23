﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Sales.Backend
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //cada vez que inicie ve si la base de datos cambia, y si es así migra la base de datos
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Models.LocalDataContext, Migrations.Configuration>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
