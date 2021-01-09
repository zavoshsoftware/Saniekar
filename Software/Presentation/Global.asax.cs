using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GSD.Globalization;
using Helpers;

namespace Presentation
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(DateTime), new PersianDateModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new PersianDateModelBinder());
        }

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    var persianCulture = new PersianCulture();
        //    Thread.CurrentThread.CurrentCulture = persianCulture;
        //    Thread.CurrentThread.CurrentUICulture = persianCulture;
        //}
    }
}
